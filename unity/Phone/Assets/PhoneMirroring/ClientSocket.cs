using UnityEngine;            // These are the libraries being used
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets; 
using System.Runtime.InteropServices;
using System.Threading;

public class ClientSocket : MonoBehaviour {

  public delegate void OnActivityChangedEvent(String componentName);
  public event OnActivityChangedEvent activityChangedListeners {
    add {
      if (value != null && currentComponentName != null) {
        value(currentComponentName);
      }

      activityChangedListenersInternal += value;
    }

    remove {
      activityChangedListenersInternal -= value;
    }
  }

  public enum ScreenOrientation {
    kPortrait = 1,
    kLandscape
  };

  public delegate void OnOrientationChangedEvent(ScreenOrientation orientation);
  public event OnOrientationChangedEvent orientationChangedListeners {
    add {
      if (value != null) {
        value(currentOrientation);
      }
      
      orientationChangedListenersInternal += value;
    }
    
    remove {
      orientationChangedListenersInternal -= value;
    }
  }

  public ScreenOrientation getScreenOrientation() {
    return currentOrientation;
  }

  public struct GyroEvent {
    public readonly Vector3 value;
    public readonly long timestamp;
    
    public GyroEvent(byte[] buffer, int offset) {
      if (BitConverter.IsLittleEndian) {
        Array.Reverse (buffer, offset, 4);
        Array.Reverse (buffer, offset + 4, 4);
        Array.Reverse (buffer, offset + 8, 4);
        Array.Reverse (buffer, offset + 12, 8);
      }
      value = new Vector3(
        BitConverter.ToSingle (buffer, offset),
        BitConverter.ToSingle (buffer, offset + 4),
        BitConverter.ToSingle (buffer, offset + 8));
      timestamp = BitConverter.ToInt64 (buffer, offset + 12);
    }
  }

  public struct AccelEvent {
    public readonly Vector3 value;
    public readonly long timestamp;
    
    public AccelEvent(byte[] buffer, int offset) {
      if (BitConverter.IsLittleEndian) {
        Array.Reverse (buffer, offset, 4);
        Array.Reverse (buffer, offset + 4, 4);
        Array.Reverse (buffer, offset + 8, 4);
        Array.Reverse (buffer, offset + 12, 8);
      }
      value = new Vector3(
        BitConverter.ToSingle (buffer, offset),
        BitConverter.ToSingle (buffer, offset + 4),
        BitConverter.ToSingle (buffer, offset + 8));
      timestamp = BitConverter.ToInt64 (buffer, offset + 12);
    }
  }

  public delegate void OnGyroEvent(GyroEvent gyroEvent);
  public event OnGyroEvent gyroEventListeners {
    add {
      if (value != null) {
        value(currentGyroEvent);
      }
      gyroEventListenersInternal += value;
    }
    
    remove {
      gyroEventListenersInternal -= value;
    }
  }

  public delegate void OnAccelEvent(AccelEvent accelEvent);
  public event OnAccelEvent accelEventListeners {
    add {
      if (value != null) {
        value(currentAccelEvent);
      }
      accelEventListenersInternal += value;
    }
    
    remove {
      accelEventListenersInternal -= value;
    }
  }

  public delegate void OnTouchEvent(InjectEventRequest touchEvent);
  public event OnTouchEvent touchEventListeners {
    add {
      if (value != null && currentTouchEvent.pointers != null /* null only during init */) {
        value (currentTouchEvent);
      }
      touchEventListenersInternal += value;
    }

    remove {
      touchEventListenersInternal -= value;
    }
  }

  // Action constants. These should match the constants in the Android MotionEvent:
  // http://developer.android.com/reference/android/view/MotionEvent.html#ACTION_CANCEL
  public enum EventAction {
    kActionDown = 0,
    kActionUp = 1,
    kActionMove = 2,
    kActionCancel = 3,
    kActionPointerDown = 5,
    kActionPointerUp = 6,
  };
  
  private Thread dataThread;
  private TcpClient dataSocket;
  private NetworkStream dataStream;

  private Thread controlThread;
  private TcpClient controlSocket;
  private NetworkStream controlStream;

  private Thread sensorThread;
  private TcpClient sensorSocket;
  private NetworkStream sensorStream;

  private static readonly int kDataPort = 7001;
  private static readonly int kControlPort = 7002;
  private static readonly int kSensorPort = 7003;

  private byte[] messageBuf;
  private IntPtr ptr;

  private volatile bool shouldStop = false;
  private volatile int nextRequestId = 0;

  string currentComponentName;
  ScreenOrientation currentOrientation;
  GyroEvent currentGyroEvent;
  AccelEvent currentAccelEvent;
  InjectEventRequest currentTouchEvent;

  private event OnActivityChangedEvent activityChangedListenersInternal;
  private event OnOrientationChangedEvent orientationChangedListenersInternal;
  private event OnGyroEvent gyroEventListenersInternal;
  private event OnAccelEvent accelEventListenersInternal;
  private event OnTouchEvent touchEventListenersInternal;
  private Dictionary<int, DateTime> fingerTimestamp = new Dictionary<int, DateTime>();

  struct ControlHeader {
    public static readonly int kSize = 4 * 3;

    public enum Magic {
      kRequest = unchecked((int) 0xFEE1CAFE),
      kResponse = unchecked((int) 0xDEADBEEF),
    };

    public enum Opcode {
      kAck = 0,    // 
      kNoop,       // Can be used as keep-alives if required. No arguments.
      kInjectEvent,    // Injects an event. See InjectEventRequest.
      kActivityChanged,  // Notifies about an Android activity change. See ActivityChangedRequest.
      kOrientationChanged, // Notifies that the Android phone's display has changed orientation. See OrientationChangedRequest.
      kGyroEvent,
      kAccelEvent,
      kTouchEvent,
    };

    public readonly Magic magic;
    public readonly int requestId;
    public readonly Opcode opcode;

    public ControlHeader(Magic magic, int requestId, Opcode opcode) {
      this.magic = magic;
      this.requestId = requestId;
      this.opcode = opcode;
    }

    public ControlHeader(byte[] buffer, int offset) {
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(buffer, offset, 4);
        Array.Reverse(buffer, offset + 4, 4);
        Array.Reverse(buffer, offset + 8, 4);
      }

      magic = (Magic) System.BitConverter.ToInt32(buffer, offset);
      requestId = System.BitConverter.ToInt32(buffer, offset + 4);
      opcode = (Opcode) System.BitConverter.ToInt32(buffer, offset + 8);
    }

    public void writeToStream(NetworkStream stream) {
      stream.Write(correctEndianness(BitConverter.GetBytes((int) magic)), 0, 4);
      stream.Write(correctEndianness(BitConverter.GetBytes(requestId)), 0, 4);
      stream.Write(correctEndianness(BitConverter.GetBytes((int) opcode)), 0, 4);
    }		
  };

  public struct InjectEventRequest {
    public static readonly int kSize = 4 * 5;

    public readonly int size;
    private readonly int action;  // use getActionMasked() and getActionPointer() instead.
    public readonly int relativeTimestamp;
    public readonly List<Pointer> pointers;

    public struct Pointer {
      public readonly int fingerId;
      public readonly float normalizedX;
      public readonly float normalizedY;

      public Pointer(int fingerId, float normalizedX, float normalizedY) {
        this.fingerId = fingerId;
        this.normalizedX = normalizedX;
        this.normalizedY = normalizedY;
      }

      public override string ToString ()
      {
        return string.Format ("({0}, {1}, {2})", fingerId, normalizedX, normalizedY);
      }
    }

    public InjectEventRequest(EventAction action, int pointerId, int relativeTimestamp, List<Pointer> pointers) {
      int fingerIndex = 0;
      if (action == EventAction.kActionPointerDown || action == EventAction.kActionPointerUp) {
        fingerIndex = findPointerIndex(pointerId, pointers);
        if (fingerIndex == -1) {
          Debug.LogWarning("Could not find specific fingerId " + pointerId
            + " in the supplied list of pointers.");
          fingerIndex = 0;
        }
      }
      this.action = getActionUnmasked(action, fingerIndex);
      this.relativeTimestamp = relativeTimestamp;
      this.pointers = pointers;
      this.size = 3 * 4 + 3 * 4 * pointers.Count;
    }

    public InjectEventRequest(byte[] buffer, int offset) {

      if (BitConverter.IsLittleEndian) {
        for (int byteIndex = 0; byteIndex < buffer.Length; byteIndex += 4) {
          Array.Reverse(buffer, offset + byteIndex, 4);
        }
      }

      size = BitConverter.ToInt32 (buffer, offset);
      action = BitConverter.ToInt32(buffer, offset + 4);
      relativeTimestamp = BitConverter.ToInt32(buffer, offset + 8);
      pointers = new List<Pointer>();
      for (int byteIndex = 12; byteIndex < buffer.Length; byteIndex += 12) {
        Pointer current = new Pointer(
          BitConverter.ToInt32 (buffer, offset + byteIndex),
          BitConverter.ToSingle(buffer, offset + byteIndex + 4),
          BitConverter.ToSingle(buffer, offset + byteIndex + 8));
        pointers.Add(current);
      }
    }

    public void writeToStream(NetworkStream stream) {
      stream.Write(correctEndianness(BitConverter.GetBytes(size)), 0, 4);
      stream.Write(correctEndianness(BitConverter.GetBytes(action)), 0, 4);
      stream.Write(correctEndianness(BitConverter.GetBytes(relativeTimestamp)), 0, 4);
      for (int i = 0; i < pointers.Count; i++) {
        stream.Write (correctEndianness (BitConverter.GetBytes (pointers[i].fingerId)), 0, 4);
        stream.Write (correctEndianness (BitConverter.GetBytes (pointers[i].normalizedX)), 0, 4);
        stream.Write (correctEndianness (BitConverter.GetBytes (pointers[i].normalizedY)), 0, 4);
      }
    }

    // See Android's getActionMasked() and getActionIndex().
    private static readonly int ACTION_POINTER_INDEX_SHIFT = 8;
    private static readonly int ACTION_POINTER_INDEX_MASK = 0xff00;
    private static readonly int ACTION_MASK = 0xff;

    public EventAction getActionMasked() {
      return (EventAction)(action & ACTION_MASK);
    }

    public Pointer getActionPointer() {
      int index = (action & ACTION_POINTER_INDEX_MASK) >> ACTION_POINTER_INDEX_SHIFT;
      return pointers[index]; 
    }


    private static int getActionUnmasked(EventAction action, int fingerIndex) {
      return ((int)action) | (fingerIndex << ACTION_POINTER_INDEX_SHIFT);
    }

    private static int findPointerIndex(int fingerId, List<Pointer> pointers) {
      // Encode the fingerId info into the action, as Android does. See Android's 
      // getActionMasked() and getActionIndex().
      int fingerIndex = -1;
      for (int i = 0; i < pointers.Count; i++) {
        if (fingerId == pointers[i].fingerId) {
          fingerIndex = i;
          break;
        }
      }

      return fingerIndex;
    }

    public override string ToString ()
    {
      System.Text.StringBuilder builder = new System.Text.StringBuilder ();
      builder.AppendFormat("t = {0}; A = {1}; P = {2}; N = {3}; [", relativeTimestamp,
                 getActionMasked (), getActionPointer ().fingerId, pointers.Count);
      for (int i = 0; i < pointers.Count; i++) {
        builder.Append(pointers[i]).Append (", ");
      }
      builder.Append ("]");
      return builder.ToString();
          
    }
  };

  struct ActivityChangedRequest {
    public readonly String componentName;

    public ActivityChangedRequest(byte[] buffer, int offset) {
      // Seems we don't need to correct endianness here.
      componentName = System.Text.Encoding.UTF8.GetString(buffer);
    }
  };

  struct OrientationChangedRequest {
    public readonly ScreenOrientation orientation;

    public OrientationChangedRequest(byte[] buffer, int offset) {
      orientation = (ScreenOrientation) buffer[offset];
    }
  }

  public void Init() {
    Debug.Log ("Setting up socket.");

    messageBuf = new byte[1000000];
    ptr = Marshal.AllocHGlobal(messageBuf.Length);
    setupPortForwarding ();

    if (Config.MIRROR_MODE != Config.Mode.OFF) {
      dataThread = new Thread(dataSocketLoop);
      dataThread.Start();

      controlThread = new Thread(controlSocketLoop);
      controlThread.Start();
    }

    if (Config.SENSORS_MODE != Config.Mode.OFF) {
      sensorThread = new Thread(sensorSocketLoop);
      sensorThread.Start();
    }
  }

  private void setupPortForwarding() {
    List<int> portsToForward = new List<int> ();
    if (Config.MIRROR_MODE == Config.Mode.USB) {
      portsToForward.Add (kDataPort);
      portsToForward.Add (kControlPort);
    }
    if (Config.SENSORS_MODE == Config.Mode.USB) {
      portsToForward.Add (kSensorPort);
    }
    if (portsToForward.Count == 0) {
      return;
    }
    string cmd = @"/k adb kill-server";
    for (int i = 0; i < portsToForward.Count; i++) {
      int port = portsToForward[i];
      cmd += " & adb forward tcp:" + port + " tcp:" + port;
    }
    cmd += " & exit";

    Debug.Log ("Executing: [" + cmd + "]");

    // This is the code for the base process
    System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
    // Start a new instance of this program but specify the 'spawned' version.
    System.Diagnostics.ProcessStartInfo myProcessStartInfo =
      new System.Diagnostics.ProcessStartInfo("CMD.exe", cmd);
    myProcessStartInfo.UseShellExecute = false;
    myProcessStartInfo.RedirectStandardOutput = true;
    myProcessStartInfo.CreateNoWindow = true;
    myProcess.StartInfo = myProcessStartInfo;
    
    myProcess.Start();
    myProcess.WaitForExit ();
    myProcess.Close ();
  }  

  private void dataSocketLoop() {
    while (!shouldStop && dataSocket == null) {
      string addr = 
          Config.MIRROR_MODE == Config.Mode.USB ? Config.USB_SERVER_IP : Config.WIFI_SERVER_IP;
      try {        
        dataSocket = new TcpClient (addr, kDataPort);
        dataStream = dataSocket.GetStream ();
        Debug.Log ("Connected to data socket: " + addr + ":" + kDataPort);
      } catch (Exception e) {
        Debug.Log ("Error connecting to data socket: " + addr + ":" + kDataPort + ". " + e);
        Thread.Sleep (1000); // 1 seconds
      }
    }

    readFileHeader ();
    
    while (!shouldStop) {
      if (dataStream != null && dataStream.DataAvailable) {
        int sz = readFrameHeader();
        if (sz == 0) {
          continue;
        }
        //Debug.Log ("Reading frame of size: " + sz);
        
        blockingRead (dataStream, messageBuf, 0, sz);
        Marshal.Copy (messageBuf, 0, ptr, sz);

        //Debug.Log ("Copied frame of size: " + sz);

        UseRenderingPlugin.DecodeFrame(ptr, sz);

        FPSDisplay.addStreamedBytes(sz + 4);

        // int renderingTimeMs = UseRenderingPlugin.DecodeFrame(ptr, sz);
        //Debug.Log ("Time spent in plugin: " + renderingTimeMs);
      }
    }
  }

  private void controlSocketLoop() {
    byte[] buffer = new byte[ControlHeader.kSize];

    while (!shouldStop && controlSocket == null) {
      string addr =
          Config.MIRROR_MODE == Config.Mode.USB ? Config.USB_SERVER_IP : Config.WIFI_SERVER_IP;
      try {
        controlSocket = new TcpClient (addr, kControlPort);
        controlStream = controlSocket.GetStream ();
        Debug.Log ("Connected to control socket: " + addr + ":" + kControlPort);
      } catch (Exception e) {
        Debug.Log ("Error connecting to control socket: " + addr + ":" + kControlPort + ". " + e);
        Thread.Sleep (1000); // 1 second
      }
    }

    while (!shouldStop) {
      if (controlStream != null && controlStream.DataAvailable) {
        int bytesRead = blockingRead(controlStream, buffer, 0, ControlHeader.kSize);
        if (bytesRead < ControlHeader.kSize)
          continue;

        ControlHeader header = new ControlHeader(buffer, 0);
        switch (header.magic) {
        case ControlHeader.Magic.kRequest:
          processControlRequest(controlStream, header.requestId, header.opcode);
          break;

        case ControlHeader.Magic.kResponse:
          processControlResponse(controlStream, header.requestId, header.opcode);
          //nextRequestId = header.requestId + 1;
          break;

        default:
          Debug.LogWarning("Unexpected control magic " + header.magic);
          continue;
        }	
      }
    }
  }

  private void sensorSocketLoop() {
    byte[] buffer = new byte[ControlHeader.kSize];
    while (!shouldStop && sensorSocket == null) {
      string addr =
          Config.SENSORS_MODE == Config.Mode.USB ? Config.USB_SERVER_IP : Config.WIFI_SERVER_IP;
      try {        
        sensorSocket = new TcpClient (addr, kSensorPort);
        sensorStream = sensorSocket.GetStream ();
        Debug.Log ("Connected to sensor socket: " + addr + ":" + kSensorPort);
      } catch (Exception e) {
        Debug.Log ("Error connecting to sensor socket: " + addr + ":" + kSensorPort + ". " + e);
        Thread.Sleep (1000); // 1 second
      }
    }
    while (!shouldStop) {
      if (sensorStream == null || !sensorStream.DataAvailable) {
        continue;
      }
      int bytesRead = blockingRead(sensorStream, buffer, 0, ControlHeader.kSize);
      if (bytesRead < ControlHeader.kSize)
        continue;
      
      ControlHeader header = new ControlHeader(buffer, 0);
      switch (header.magic) {
      case ControlHeader.Magic.kRequest:
        processSensorRequest(sensorStream, header.requestId, header.opcode);
        if (Config.TRACK_ROUNDTRIP_LATENCY) {
          sendAck(sensorStream, header.requestId);
        }
        break;
        
      case ControlHeader.Magic.kResponse:
        // TODO: process message responses if needed.
        // For now our all our messages are single-directed.
        Debug.LogWarning("Response opcode received: " + header.opcode);
        //nextRequestId = header.requestId + 1;
        break;
        
      default:
        Debug.LogWarning("Unexpected control magic " + header.magic);
        continue;
      }	
    }
  }

  private int blockingRead(NetworkStream stream, byte[] buffer, int index, int count) {
    int bytesRead = 0;
    while (!shouldStop && bytesRead < count) {
      if (stream == null || !stream.DataAvailable) {
        continue;
      }
      bytesRead += stream.Read (buffer, index + bytesRead, count - bytesRead);
    }
    return bytesRead;
  }
  
  private int readFileHeader() {
    byte[] buf = new byte[32];
    int bytesRead = blockingRead (dataStream, buf, 0, 32);
    //Debug.LogWarning ("readFileHeader read: " + System.BitConverter.ToString (buf));
    return bytesRead;
  }

  // Returns the number of bytes in the frame.
  private int readFrameHeader() {
    byte[] header = new byte[12];
    
    blockingRead (dataStream, header, 0, 12);
    int sz = unpack32bits (header, 0);
    long timestamp = unpack64bits (header, 4);
    
    //Debug.Log ("Read frame header: " + System.BitConverter.ToString (header) + " with size: " + sz);
    return sz;
  }

  void OnDestroy() {
    shouldStop = true;
    if (dataThread != null) {
      dataThread.Join();
    }
    if (controlThread != null) {
      controlThread.Join();
    }
    if (sensorThread != null) {
      sensorThread.Join();
    }

    if (dataSocket != null) {
      dataSocket.Close ();
      dataSocket = null;
    }
    if (controlSocket != null) {
      controlSocket.Close ();
      controlSocket = null;
    }
    if (sensorSocket != null) {
      sensorSocket.Close ();
      sensorSocket = null;
    }
  }
  
  private int unpack32bits(byte[] array, int offset) {
    int num = 0;
    for (int i = 0; i < 4; i++) {
      num += array [offset + i] << (i * 8);
    }
    return num;
  }
  
  private long unpack64bits(byte[] array, int offset) {
    int num = 0;
    for (int i = 0; i < 8; i++) {
      num += array [offset + i] << (i * 8);
    }
    return num;
  }

  private void processSensorRequest(NetworkStream stream, int requestId, ControlHeader.Opcode opcode) {
    switch (opcode) {
    case ControlHeader.Opcode.kNoop:
      break;
    case ControlHeader.Opcode.kGyroEvent: {
      byte[] buffer = new byte[3 * 4 + 8];
      if (blockingRead(stream, buffer, 0, buffer.Length) < buffer.Length) {
        Debug.LogError("Invalid gyro event message");
        break;
      }
      
      onGyroEvent(requestId, new GyroEvent(buffer, 0));
      break;
    }
    case ControlHeader.Opcode.kAccelEvent: {
      byte[] buffer = new byte[3 * 4 + 8];
      if (blockingRead(stream, buffer, 0, buffer.Length) < buffer.Length) {
        Debug.LogError("Invalid accel event message");
        break;
      }
      onAccelEvent(requestId, new AccelEvent(buffer, 0));
      break;
    }
    case ControlHeader.Opcode.kTouchEvent: {
      // Read the size.
      byte[] sizeBuf = new byte[4];
      if (blockingRead(stream, sizeBuf, 0, 4) < 4) {
        Debug.LogError ("Unable to read inject event length.");
        return;
      }
      int msgSize = BitConverter.ToInt32 (correctEndianness(sizeBuf), 0);

      // Read the rest of the message.
      byte[] buffer  = new byte[msgSize];
      buffer[0] = sizeBuf[0];
      buffer[1] = sizeBuf[1];
      buffer[2] = sizeBuf[2];
      buffer[3] = sizeBuf[3];

      if (blockingRead(stream, buffer, 4, msgSize - 4) < msgSize - 4) {
        Debug.LogError("Invalid inject event message");
        return;
      }
      
      onInjectEvent(requestId, new InjectEventRequest(buffer, 0));
      break;
    }
    default:
      break;
    }

  }

  // For latency testing. Respond to ping.
  private static void sendAck(NetworkStream stream, int requestId) {
    ControlHeader header = new ControlHeader (ControlHeader.Magic.kResponse, requestId, ControlHeader.Opcode.kAck);
    header.writeToStream (stream);
    stream.Flush ();
  }

  RoundTripEstimator injectionRT = Config.TRACK_ROUNDTRIP_LATENCY ? new RoundTripEstimator() : null;

  private void processControlResponse(NetworkStream stream, int requestId, ControlHeader.Opcode opcode) {
    switch (opcode) {
      case ControlHeader.Opcode.kAck:
        if (Config.TRACK_ROUNDTRIP_LATENCY) {
          injectionRT.recordReceived(requestId);
          FPSDisplay.avgInjectionRT = injectionRT.getAvg();
          FPSDisplay.maxInjectionRT = injectionRT.getMax();
        }
        break;
      default:
        Debug.LogError("Unexpected control response. Opcode: " + opcode);
        break;
    }
  }

  private void processControlRequest(NetworkStream stream, int requestId, ControlHeader.Opcode opcode) {
    switch (opcode) {
    case ControlHeader.Opcode.kNoop:
      break;
      
    case ControlHeader.Opcode.kInjectEvent: {
      Debug.LogError("Unexpected touch event streamed in control stream.");
      break;
      /*
      byte[] buffer  = new byte[InjectEventRequest.kSize];
      if (blockingRead(stream, buffer, 0, InjectEventRequest.kSize) < InjectEventRequest.kSize) {
        Debug.LogError("Invalid inject event message");
        return;
      }
      
      onInjectEvent(requestId, new InjectEventRequest(buffer, 0));
      break;
       */
    }
      
    case ControlHeader.Opcode.kActivityChanged: {
      byte[] lenBuffer = new byte[4];
      if (blockingRead(stream, lenBuffer, 0, 4) != 4) {
        Debug.LogError("Invalid activity changed message");
      }
      
      if (BitConverter.IsLittleEndian)
        Array.Reverse(lenBuffer, 0, 4);
      
      int stringBufferLength = BitConverter.ToInt32(lenBuffer, 0);
      byte[] buffer  = new byte[stringBufferLength];
      if (blockingRead(stream, buffer, 0, stringBufferLength) < stringBufferLength) {
        Debug.LogError("Could not read the full component name");
        return;
      }
      
      onActivityChanged(requestId, new ActivityChangedRequest(buffer, 0));
      break;
    }

    case ControlHeader.Opcode.kOrientationChanged: {
      byte[] buffer = new byte[1];
      if (blockingRead(stream, buffer, 0, 1) < 1) {
        Debug.LogError("Could not read orientation byte");
        return;
      }
      
      onOrientationChanged(requestId, new OrientationChangedRequest(buffer, 0));
      break;
    }
      
    default:
      Debug.LogWarning("Unexpected control opcode " + opcode);
      break;
    }
  }

  private void onInjectEvent(int requestId, InjectEventRequest request) {
    FPSDisplay.recordTouchEvent();
    currentTouchEvent = request;
    if (touchEventListenersInternal != null) {
      touchEventListenersInternal (request);
    }
  }
  
  private void onActivityChanged(int requestId, ActivityChangedRequest request) {
    currentComponentName = request.componentName;
    if (activityChangedListenersInternal != null && request.componentName != null) {
      activityChangedListenersInternal(request.componentName);
    }
  }

  private void onOrientationChanged(int requestId, OrientationChangedRequest request) {
    currentOrientation = request.orientation;
    if (orientationChangedListenersInternal != null) {
      orientationChangedListenersInternal (request.orientation);
    }
  }

  private void onGyroEvent(int requestId, GyroEvent request) {
    currentGyroEvent = request;
    if (gyroEventListenersInternal != null) {
      gyroEventListenersInternal (request);
    }
  }

  private void onAccelEvent(int requestId, AccelEvent request) {
    currentAccelEvent = request;
    if (accelEventListenersInternal != null) {
      accelEventListenersInternal (request);
    }
  }
  
  // For multi-touch.
  // Note that you need to provide fingerId of the finger going up or down, when action
  // is ACTION_POINTER_UP or ACTION_POINTER_DOWN.
  public void injectEvent(EventAction action, int fingerId, List<InjectEventRequest.Pointer> pointers) {
    int relativeTimestamp = 0;
    if (action == EventAction.kActionDown) {
      fingerTimestamp.Add(fingerId, DateTime.Now);
    } else {
      DateTime startTimestamp;
      if (fingerTimestamp.TryGetValue(fingerId, out startTimestamp))
        relativeTimestamp = (int) (DateTime.Now - startTimestamp).TotalMilliseconds;
      
      if (action == EventAction.kActionUp)
        fingerTimestamp.Remove(fingerId);
    }

    injectEvent (action, fingerId, pointers, relativeTimestamp);
  }
  
  // For touch-forwarding.
  public void injectEvent(InjectEventRequest motionEvent) {
    EventAction action = motionEvent.getActionMasked ();
    int fingerId = 0;
    if (action == EventAction.kActionPointerDown || action == EventAction.kActionPointerUp) {
      fingerId = motionEvent.getActionPointer ().fingerId;
    }
    injectEvent(action, fingerId, motionEvent.pointers, motionEvent.relativeTimestamp);
  }

  // For single-touch.
  public void injectEvent(EventAction action, int fingerId, float normalizedX, float normalizedY) {
    // Debug.Log ("Injecting " + normalizedX + ", " + normalizedY);
    
    List<InjectEventRequest.Pointer> pointers = new List<InjectEventRequest.Pointer>();
    pointers.Add(new InjectEventRequest.Pointer(fingerId, normalizedX, normalizedY));
    injectEvent(action, fingerId, pointers);
  }

  private void injectEvent(EventAction action, int fingerId,
      List<InjectEventRequest.Pointer> pointers, int relativeTimestamp) {
    ControlHeader header = new ControlHeader(ControlHeader.Magic.kRequest, ++nextRequestId, 
                         ControlHeader.Opcode.kInjectEvent);

    if (injectionRT != null) {
      injectionRT.recordSent(header.requestId);
    }
    
    InjectEventRequest injectEvent = new InjectEventRequest(action, fingerId, relativeTimestamp, pointers);
    
    //Debug.Log("injecting: " + injectEvent.ToString());
    
    header.writeToStream(controlStream);
    injectEvent.writeToStream(controlStream);
    controlStream.Flush();
  }
  
  static private byte[] correctEndianness(byte[] array) {
    if (BitConverter.IsLittleEndian)
      Array.Reverse(array);
    
    return array;
  }
} 

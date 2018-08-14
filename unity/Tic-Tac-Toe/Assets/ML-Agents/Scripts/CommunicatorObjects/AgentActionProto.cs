// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: communicator_objects/agent_action_proto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace MLAgents.CommunicatorObjects {

  /// <summary>Holder for reflection information generated from communicator_objects/agent_action_proto.proto</summary>
  public static partial class AgentActionProtoReflection {

    #region Descriptor
    /// <summary>File descriptor for communicator_objects/agent_action_proto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AgentActionProtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci1jb21tdW5pY2F0b3Jfb2JqZWN0cy9hZ2VudF9hY3Rpb25fcHJvdG8ucHJv",
            "dG8SFGNvbW11bmljYXRvcl9vYmplY3RzImEKEEFnZW50QWN0aW9uUHJvdG8S",
            "FgoOdmVjdG9yX2FjdGlvbnMYASADKAISFAoMdGV4dF9hY3Rpb25zGAIgASgJ",
            "EhAKCG1lbW9yaWVzGAMgAygCEg0KBXZhbHVlGAQgASgCQh+qAhxNTEFnZW50",
            "cy5Db21tdW5pY2F0b3JPYmplY3RzYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::MLAgents.CommunicatorObjects.AgentActionProto), global::MLAgents.CommunicatorObjects.AgentActionProto.Parser, new[]{ "VectorActions", "TextActions", "Memories", "Value" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class AgentActionProto : pb::IMessage<AgentActionProto> {
    private static readonly pb::MessageParser<AgentActionProto> _parser = new pb::MessageParser<AgentActionProto>(() => new AgentActionProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AgentActionProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::MLAgents.CommunicatorObjects.AgentActionProtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AgentActionProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AgentActionProto(AgentActionProto other) : this() {
      vectorActions_ = other.vectorActions_.Clone();
      textActions_ = other.textActions_;
      memories_ = other.memories_.Clone();
      value_ = other.value_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AgentActionProto Clone() {
      return new AgentActionProto(this);
    }

    /// <summary>Field number for the "vector_actions" field.</summary>
    public const int VectorActionsFieldNumber = 1;
    private static readonly pb::FieldCodec<float> _repeated_vectorActions_codec
        = pb::FieldCodec.ForFloat(10);
    private readonly pbc::RepeatedField<float> vectorActions_ = new pbc::RepeatedField<float>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<float> VectorActions {
      get { return vectorActions_; }
    }

    /// <summary>Field number for the "text_actions" field.</summary>
    public const int TextActionsFieldNumber = 2;
    private string textActions_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string TextActions {
      get { return textActions_; }
      set {
        textActions_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "memories" field.</summary>
    public const int MemoriesFieldNumber = 3;
    private static readonly pb::FieldCodec<float> _repeated_memories_codec
        = pb::FieldCodec.ForFloat(26);
    private readonly pbc::RepeatedField<float> memories_ = new pbc::RepeatedField<float>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<float> Memories {
      get { return memories_; }
    }

    /// <summary>Field number for the "value" field.</summary>
    public const int ValueFieldNumber = 4;
    private float value_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AgentActionProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AgentActionProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!vectorActions_.Equals(other.vectorActions_)) return false;
      if (TextActions != other.TextActions) return false;
      if(!memories_.Equals(other.memories_)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Value, other.Value)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= vectorActions_.GetHashCode();
      if (TextActions.Length != 0) hash ^= TextActions.GetHashCode();
      hash ^= memories_.GetHashCode();
      if (Value != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Value);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      vectorActions_.WriteTo(output, _repeated_vectorActions_codec);
      if (TextActions.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(TextActions);
      }
      memories_.WriteTo(output, _repeated_memories_codec);
      if (Value != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(Value);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += vectorActions_.CalculateSize(_repeated_vectorActions_codec);
      if (TextActions.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TextActions);
      }
      size += memories_.CalculateSize(_repeated_memories_codec);
      if (Value != 0F) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AgentActionProto other) {
      if (other == null) {
        return;
      }
      vectorActions_.Add(other.vectorActions_);
      if (other.TextActions.Length != 0) {
        TextActions = other.TextActions;
      }
      memories_.Add(other.memories_);
      if (other.Value != 0F) {
        Value = other.Value;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10:
          case 13: {
            vectorActions_.AddEntriesFrom(input, _repeated_vectorActions_codec);
            break;
          }
          case 18: {
            TextActions = input.ReadString();
            break;
          }
          case 26:
          case 29: {
            memories_.AddEntriesFrom(input, _repeated_memories_codec);
            break;
          }
          case 37: {
            Value = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code

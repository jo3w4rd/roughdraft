using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using System.Diagnostics;
using Unity.Burst;

namespace Samples.HelloCube_02
{
    [NativeContainer]
    [NativeContainerIsAtomicWriteOnly]
    [NativeContainerSupportsDeallocateOnJobCompletion]
    unsafe public struct NativeResult<T> : IDisposable where T : struct
    {
        [NativeDisableUnsafePtrRestriction]
        private IntPtr m_Buffer;
        internal Allocator m_AllocatorLabel;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
    [NativeSetClassTypeToNullOnSchedule]
    DisposeSentinel m_DisposeSentinel;
    internal AtomicSafetyHandle       m_Safety;
#endif

        public NativeResult(Allocator label)
        {
            m_AllocatorLabel = label;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        if (m_AllocatorLabel <= Allocator.None)
            throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
        if (!UnsafeUtility.IsBlittable<T>())
            throw new ArgumentException(string.Format("{0} used in NativeTesult<{0}> must be blittable", typeof(T)));
#endif

            m_Buffer = (IntPtr) UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), label);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0, m_AllocatorLabel);
#endif
        }

        public T Result
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                T result = new T();
                UnsafeUtility.CopyPtrToStructure((void*)m_Buffer, out result);
                return result;
            }
            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
                UnsafeUtility.CopyStructureToPtr(ref value, (void*)m_Buffer);

            }
        }

        public bool IsCreated
        {
            get { return m_Buffer != null; }
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#endif

            UnsafeUtility.Free((void*) m_Buffer, m_AllocatorLabel);
            m_Buffer = IntPtr.Zero;
        }

    }
}
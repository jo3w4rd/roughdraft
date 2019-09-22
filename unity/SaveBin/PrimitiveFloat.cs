using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using System.Diagnostics;
using Unity.Burst;

namespace Samples.HelloCube_02
{
    [StructLayout(LayoutKind.Sequential)]
    [NativeContainer]
    [NativeContainerIsAtomicWriteOnly]
    unsafe public struct PrimitiveFloat : IDisposable
    {
        [NativeDisableUnsafePtrRestriction]
        private float* m_value;
        private Allocator allocator;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle m_Safety;
        [NativeSetClassTypeToNullOnSchedule]
        DisposeSentinel m_DisposeSentinel;
#endif

        public PrimitiveFloat(Allocator allocator)
        {
            this.allocator = allocator;
            m_value = (float*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<float>(), 4, allocator);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0, allocator);
#endif

            Value = 0;
        }

        public float Value
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                      AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                return *m_value;
            }
            set
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
                *m_value = value;

            }
        }

        public bool IsCreated
        {
            get { return m_value != null; }
        }

        public void Dispose()
        {
            // Let the dispose sentinel know that the data has been freed so it does not report any memory leaks
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#endif

            UnsafeUtility.Free(m_value, allocator);
            m_value = null;
        }
    }
}
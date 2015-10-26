using System;
using System.Runtime.InteropServices;

namespace System.Windows.Media.Imaging
{
	internal static class IntPtrExtender
	{
		/// <summary>
		/// Add offset (size of type T * count) to the value of a pointer to T.
		/// </summary>
		/// <param name="ptr">The pointer to add to.</param>
		/// <param name="count">Number of T's to offset the pointer by.</param>
		/// <returns>A new pointer that reflects the offset to the pointer.</returns>
		public static IntPtr Add<T>( this IntPtr ptr, int count )
		{
			int offset = Marshal.SizeOf( typeof( T ) ) * count;
			return ( IntPtr.Add( ptr, offset ) );
		}
	}
}

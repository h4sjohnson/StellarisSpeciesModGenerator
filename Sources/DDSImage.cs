using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
public class DDSImage 
{
    private static readonly int HeaderSize=128;
    DDSHeader _header;
    byte[] rawData;
    
    public DDSHeader Header{get{ return _header;}} 
    public HeaderFlags headerFlags{get{return Header.Flags;}}
    public bool IsValid{get{return Header.IsValid&&Header.SizeCheck();}}
    bool CheckFlag(HeaderFlags flag){return (Header.Flags&flag)==flag;}
    public bool HasHeight{get{return CheckFlag(HeaderFlags.HEIGHT);}}
    public int Height{get{return (int)Header.Height;}}
    public bool HasWidth{get{return CheckFlag(HeaderFlags.WIDTH);}}
    public int Width{get{return (int)Header.Width;}}
    public bool HasDepth{get{return CheckFlag(HeaderFlags.DEPTH);}}
    public int Depth{get{return (int)Header.Depth;}}
    public int MipMapCount{get{return (int)Header.MipMapCount;}}
    public bool HasFourCC{get{return Header.PixelFormat.dwFlags==PixelFormatFlags.FOURCC;}}
    public bool IsUncompressedRGB{get{return Header.PixelFormat.dwFlags==PixelFormatFlags.RGB;}}
    public DDSImage(byte[] rawData){
        this.rawData=rawData;
        _header=ByteArrayToStructure<DDSHeader>(rawData);
    }
    
    public byte[] GetTextureData(){
        byte[] texData = new byte[rawData.Length-HeaderSize];
        GetRGBData(texData);
        return texData;
    }

    public void GetRGBData(byte[] rgbData){
        System.Buffer.BlockCopy(rawData,HeaderSize,rgbData,0,rgbData.Length);
    }

    static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
    {
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        try {
            return (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        }
        finally {
            handle.Free();
        }
    }
     
    [StructLayout(LayoutKind.Sequential)]
    public struct DDSHeader{
        public UInt32 magicWord;
        public bool IsValid{get{return magicWord==0x20534444;}}
        //magicWord is not included in the header size
        public UInt32 size;
        public bool SizeCheck(){return size==124;}
        public HeaderFlags Flags;
        public UInt32 Height;
        public UInt32           Width;
        public UInt32           PitchOrLinearSize;
        public UInt32           Depth;
        public UInt32           MipMapCount;
  //11
        public UInt32           Reserved1;
        public UInt32           Reserved2;
        public UInt32           Reserved3;
        public UInt32           Reserved4;
        public UInt32           Reserved5;
        public UInt32           Reserved6;
        public UInt32           Reserved7;
        public UInt32           Reserved8;
        public UInt32           Reserved9;
        public UInt32           Reserved10;
        public UInt32           Reserved11;
        public DDSPixelFormat PixelFormat;
        public UInt32           dwCaps;
        public UInt32           dwCaps2;
        public UInt32           dwCaps3;
        public UInt32           dwCaps4;
        public UInt32           dwReserved2;

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DDSPixelFormat{
        public UInt32 size;
        public bool SizeCheck(){return size==32;}
        public PixelFormatFlags dwFlags;
        //string
        public UInt32 FourCC;
        public UInt32 dwRGBBitCount;
        public UInt32 dwRBitMask;
        public UInt32 dwGBitMask;
        public UInt32 dwBBitMask;
        public UInt32 dwABitMask;

    }
    [Flags]
    public enum HeaderFlags{
        CAPS=0x1,
        HEIGHT=0x2,
        WIDTH=0x4,
        PITCH=0x8,
        PIXELFORMAT=0x1000,
        MIPMAPCOUNT=0x20000,
        LINEARSIZE=0x80000,
        DEPTH=0x800000,
        TEXTURE= CAPS | HEIGHT | WIDTH | PIXELFORMAT,
    }
    [Flags]
    public enum PixelFormatFlags{
        ALPHAPIXELS=0x1,
        ALPHA=0x2,
        FOURCC=0x4,
        RGB=0x40,
        YUV=0x200,
        LUMINANCE=0x20000
    }
}

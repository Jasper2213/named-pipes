using System.Text;

namespace DAL.Domain;

public class StreamString {
    private readonly Stream _ioStream;
    private readonly UnicodeEncoding _streamEncoding = new();

    public StreamString(Stream ioStream) {
        _ioStream = ioStream;
    }

    public string ReadString() {
        var lenBuffer = new byte[sizeof(int)];
        _ioStream.Read(lenBuffer, 0, sizeof(int));
        var stringLen = BitConverter.ToInt32(lenBuffer, 0);

        var inBuffer = new byte[stringLen];
        _ioStream.Read(inBuffer, 0, stringLen);

        return _streamEncoding.GetString(inBuffer);
    }

    public int WriteString(string outString) {
        var outBuffer = _streamEncoding.GetBytes(outString);
        var len = outBuffer.Length;
        if (len > ushort.MaxValue) {
            len = ushort.MaxValue;
        }

        var lenBuffer = BitConverter.GetBytes(len);
        _ioStream.Write(lenBuffer, 0, sizeof(int));
        _ioStream.Write(outBuffer, 0, len);
        _ioStream.Flush();

        return outBuffer.Length + sizeof(int);
    }
}

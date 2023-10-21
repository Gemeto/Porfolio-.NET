using System.Security.Cryptography;
using System.Text;

namespace WebApplicationTest.Services
{
    public class EncryptSHA256
    {
        private readonly ASCIIEncoding _encoding;
        private readonly StringBuilder _sb;
        private readonly SHA256 _sha256;
        public EncryptSHA256()
        {
            _encoding = new ASCIIEncoding();
            _sb = new StringBuilder();
            _sha256 = SHA256.Create();
        }
        public string GetSHA256(string str)
        {
            byte[] stream = null;
            stream = _sha256.ComputeHash(_encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) _sb.AppendFormat("{0:x2}", stream[i]);
            return _sb.ToString();
        }

    }
}

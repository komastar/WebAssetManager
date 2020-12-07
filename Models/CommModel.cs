using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetWebManager.Models
{
    public class CommModel
    {
        public byte[] Code = new byte[4];
        public int TestNumber;

        public CommModel()
        {
        }

        public void FromBytes(byte[] data)
        {
            int index = 0;
            Array.Copy(data, 0, Code, 0, 4);
            index += Code.Length;
            TestNumber = BitConverter.ToInt32(data, index);
        }

        public void Print()
        {
            Console.WriteLine(Encoding.ASCII.GetString(Code));
            Console.WriteLine(TestNumber);
        }
    }
}

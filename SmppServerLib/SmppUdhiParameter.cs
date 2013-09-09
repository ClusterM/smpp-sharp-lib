using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppUdhiParameter
    {
        public readonly byte Tag;
        public readonly byte Length;
        public readonly byte[] Value;

        public SmppUdhiParameter(byte tag, byte length, byte[] value)
        {
            Tag = tag;
            Length = length;
            Value = value;
        }

        public override string ToString()
        {
            var data = new StringBuilder();
            foreach(var b in Value)
                data.AppendFormat("{0:X2} ",b);
            return string.Format("UDHI: {0} = {1}", Tag, data.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Common.Logging;
using System.IO;
using System.Text;
using System.Collections;
namespace WebApplication2
{
    //public class GameResponse
    //{
    //    private static readonly ILog Logger = LogManager.GetLogger(typeof(GameResponse));
    //    private MemoryStream outputStream = new MemoryStream();
    //    [JsonIgnore]
    //    public List<object> ResponseList { get; private set; }
    //    private static Type responseContractType = typeof(ResponseContractAttribute);
    //    public int StatusCode { get; set; }
    //    public string Description { get; set; }
    //    public int ActionId { get; set; }
    //    public int RmId { get; set; }
    //    public string St { get; set; }
    //    //[JsonIgnore]
    //    public object ResponseObject { get; set; }
    //    [JsonIgnore]
    //    public string DescResourceId { get; set; }
    //    [JsonIgnore]
    //    public object[] FormatterArgs { get; set; }
    //    [JsonIgnore]
    //    public Stream OutputStream { get { return outputStream; } }

    //    public GameResponse()
    //    {
    //        ResponseList = new List<object>();
    //        Description = St = "";
    //        StatusCode = 10000;
    //    }

    //    public void Write(object obj)
    //    {
    //        ResponseList.Add(obj);
    //    }

    //    public byte[] ToArray()
    //    {
    //        try
    //        {
    //            //包总长度
    //            int len = 0;
    //            long pos = outputStream.Position;
    //            //包总长度，占位
    //            WriteValue(len);
    //            WriteValue(StatusCode);
    //            WriteValue(RmId);
    //            WriteValue(Description);
    //            WriteValue(ActionId);
    //            WriteValue(St);
    //            var enumable = ResponseObject as IEnumerable;
    //            if (enumable != null)
    //            {
    //                foreach (var obj in enumable)
    //                {
    //                    WriteObject(obj, false);
    //                }
    //            }
    //            else
    //            {
    //                WriteValue(0);
    //                WriteValue(0);
    //            }
    //            outputStream.Seek(pos, SeekOrigin.Begin);
    //            WriteValue((int)outputStream.Length);
    //            var result = outputStream.ToArray();
    //            outputStream.Close();
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusCode = 10002;
    //            Description = "服务器异常。";
    //            Logger.Error("ToArray", ex);
    //            outputStream.SetLength(0);
    //            ResponseObject = null;
    //            return ToArray();
    //        }
    //    }

    //    private void WriteValue(byte value)
    //    {
    //        outputStream.WriteByte(value);
    //    }
    //    private void WriteValue(short value)
    //    {
    //        //byte[] buf = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
    //        byte[] buf = BitConverter.GetBytes(value);
    //        outputStream.Write(buf, 0, buf.Length);
    //    }
    //    private void WriteValue(int value)
    //    {
    //        //byte[] buf = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
    //        byte[] buf = BitConverter.GetBytes(value);
    //        outputStream.Write(buf, 0, buf.Length);
    //    }
    //    private void WriteValue(long value)
    //    {
    //        //byte[] buf = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
    //        byte[] buf = BitConverter.GetBytes(value);
    //        outputStream.Write(buf, 0, buf.Length);
    //    }
    //    private void WriteValue(float value)
    //    {
    //        byte[] buf = BitConverter.GetBytes(value);
    //        outputStream.Write(buf, 0, buf.Length);
    //    }
    //    private void WriteValue(string value)
    //    {
    //        byte[] buf = Encoding.UTF8.GetBytes(value);
    //        WriteValue(buf.Length);
    //        outputStream.Write(buf, 0, buf.Length);
    //    }
    //    private void WriteObject(object value, bool isInArray)
    //    {
    //        if (value == null)
    //        {
    //            WriteValue((int)0);
    //            return;
    //        }
    //        Type type = value.GetType();
    //        TypeCode typecode = Type.GetTypeCode(type);



    //        switch (typecode)
    //        {
    //            case TypeCode.Boolean:
    //            case TypeCode.Byte:
    //                WriteValue(Convert.ToByte(value));
    //                break;
    //            case TypeCode.Double:
    //            case TypeCode.Decimal:
    //            case TypeCode.Single:
    //                WriteValue(Convert.ToSingle(value));
    //                break;
    //            case TypeCode.Int16:
    //            case TypeCode.UInt16:
    //                WriteValue(Convert.ToInt16(value));
    //                break;
    //            case TypeCode.Int32:
    //            case TypeCode.UInt32:
    //                WriteValue(Convert.ToInt32(value));
    //                break;
    //            case TypeCode.Int64:
    //            case TypeCode.UInt64:
    //                WriteValue(Convert.ToInt64(value));
    //                break;
    //            case TypeCode.String:
    //                WriteValue((string)value);
    //                break;
    //            case TypeCode.Object:
    //                if (value is IList)
    //                {
    //                    var realvalue = value as IList;
    //                    WriteValue(realvalue.Count);
    //                    if (realvalue.Count != 0)
    //                    {
    //                        foreach (var inst in realvalue)
    //                        {
    //                            WriteObject(inst, true);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (!isInArray) WriteValue((int)1);
    //                    long startPos = outputStream.Position;
    //                    WriteValue((int)0);
    //                    var obj = type.GetCustomAttributes(responseContractType, false);
    //                    if (obj == null || obj.Length != 1) throw new ApplicationException(string.Format("类[{0}]未标注ResponseContractAttribute", type.FullName));
    //                    var attr = obj[0] as ResponseContractAttribute;
    //                    foreach (var p in attr.Tags)
    //                    {
    //                        var prop = type.GetProperty(p);
    //                        if (prop == null) throw new ApplicationException(string.Format("类[{0}]没有属性[{1}]", type.FullName, p));
    //                        WriteObject(prop.FastGetValue(value), false);
    //                    }
    //                    long endPos = outputStream.Position;

    //                    outputStream.Seek(startPos, SeekOrigin.Begin);
    //                    WriteValue((int)(endPos - startPos));
    //                    outputStream.Seek(endPos, SeekOrigin.Begin);
    //                }
    //                break;
    //        }
    //    }
    //}
}
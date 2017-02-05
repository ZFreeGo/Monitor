using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZFreeGo.FileOperation.Comtrade;
using ZFreeGo.FileOperation.Comtrade.ConfigContent;
using ZFreeGo.FileOperation.Comtrade.DataContent;

namespace ConsoleTest
{


    class Program
    {
        static void Main(string[] args)
        {
            RijndaelManaged key = null;

            try
            {
                // Create a new Rijndael key.
                key = new RijndaelManaged();
                // Load an XML document.
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("test.xml");

                // Encrypt the "creditcard" element.
                Encrypt(xmlDoc, "creditcard", key);

                Console.WriteLine("The element was encrypted");

                Console.WriteLine(xmlDoc.InnerXml);

                Decrypt(xmlDoc, key);

                Console.WriteLine("The element was decrypted");

                Console.WriteLine(xmlDoc.InnerXml);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            finally
            {
                // Clear the key.
                if (key != null)
                {
                    key.Clear();
                }
            }

        }

        public static void Encrypt(XmlDocument Doc, string ElementName, SymmetricAlgorithm Key)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementName == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (Key == null)
                throw new ArgumentNullException("Alg");

            ////////////////////////////////////////////////
            // Find the specified element in the XmlDocument
            // object and create a new XmlElemnt object.
            ////////////////////////////////////////////////
            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementName)[0] as XmlElement;
            // Throw an XmlException if the element was not found.
            if (elementToEncrypt == null)
            {
                throw new XmlException("The specified element was not found");

            }

            //////////////////////////////////////////////////
            // Create a new instance of the EncryptedXml class 
            // and use it to encrypt the XmlElement with the 
            // symmetric key.
            //////////////////////////////////////////////////

            EncryptedXml eXml = new EncryptedXml();

            byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, Key, false);
            ////////////////////////////////////////////////
            // Construct an EncryptedData object and populate
            // it with the desired encryption information.
            ////////////////////////////////////////////////

            EncryptedData edElement = new EncryptedData();
            edElement.Type = EncryptedXml.XmlEncElementUrl;

            // Create an EncryptionMethod element so that the 
            // receiver knows which algorithm to use for decryption.
            // Determine what kind of algorithm is being used and
            // supply the appropriate URL to the EncryptionMethod element.

            string encryptionMethod = null;

            if (Key is TripleDES)
            {
                encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
            }
            else if (Key is DES)
            {
                encryptionMethod = EncryptedXml.XmlEncDESUrl;
            }
            if (Key is Rijndael)
            {
                switch (Key.KeySize)
                {
                    case 128:
                        encryptionMethod = EncryptedXml.XmlEncAES128Url;
                        break;
                    case 192:
                        encryptionMethod = EncryptedXml.XmlEncAES192Url;
                        break;
                    case 256:
                        encryptionMethod = EncryptedXml.XmlEncAES256Url;
                        break;
                }
            }
            else
            {
                // Throw an exception if the transform is not in the previous categories
                throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
            }

            edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);

            // Add the encrypted element data to the 
            // EncryptedData object.
            edElement.CipherData.CipherValue = encryptedElement;

            ////////////////////////////////////////////////////
            // Replace the element from the original XmlDocument
            // object with the EncryptedData element.
            ////////////////////////////////////////////////////
            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
        }

        public static void Decrypt(XmlDocument Doc, SymmetricAlgorithm Alg)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (Alg == null)
                throw new ArgumentNullException("Alg");

            // Find the EncryptedData element in the XmlDocument.
            XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;

            // If the EncryptedData element was not found, throw an exception.
            if (encryptedElement == null)
            {
                throw new XmlException("The EncryptedData element was not found.");
            }


            // Create an EncryptedData object and populate it.
            EncryptedData edElement = new EncryptedData();
            edElement.LoadXml(encryptedElement);

            // Create a new EncryptedXml object.
            EncryptedXml exml = new EncryptedXml();


            // Decrypt the element using the symmetric key.
            byte[] rgbOutput = exml.DecryptData(edElement, Alg);

            // Replace the encryptedData element with the plaintext XML element.
            exml.ReplaceData(encryptedElement, rgbOutput);

        }


    }




    class ProgramA
    {

        static void AMain(string[] args)
        {
            //TestStationRev();
            //TestChannelNumType();
            //TestAnalogChannelInformation();
            //TestDigitalChannelInformation();
            //TestDataStamp();
            //TestComtradeConfig();
            //TestASCII();
            //TestBinary();
            //TestComtradeManager();
        }






        static void TestComtradeManager()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("回车开始");
                    Console.ReadLine();

                    Console.WriteLine("TestComtradeManager-TestStart");
                    string path = @"E:\WorkProject\04-FTU终端\Comtrade\ComtradeFile";
                    string cfgPath = Path.Combine(path, "binary.cfg");
                    string dataPath = Path.Combine(path, "binary.dat");
                    Console.WriteLine("In:");
                    Console.WriteLine(path);
                    Console.WriteLine(cfgPath);
                    Console.WriteLine(dataPath);

                    var comtradeManager = new ComtradeFileManager();
                    comtradeManager.ReadFile(cfgPath, dataPath);


                    Console.WriteLine("Out:");
                    Console.WriteLine("配置文件内容:");
                    string[] cfgRow;
                    comtradeManager.ConfigFile.MakeConfigFile(out cfgRow);



                    for (int index = 0; index < cfgRow.Length; index++)
                    {
                        Console.Write(string.Format("{0}：" + cfgRow[index], index + 1));

                    }
                    Console.WriteLine("数据文件内容:");
                    for (int i = 0; i < comtradeManager.DataFile.AsciiData.Count; i++)
                    {
                        var ascii = new ASCIIContent(comtradeManager.DataFile.AsciiData[i],
                            comtradeManager.ConfigFile.RowChannelNumType.AnalogChannelCount,
                            comtradeManager.ConfigFile.RowChannelNumType.DigitalChannelCount);

                        Console.Write(ascii.RowToString());
                    }
                    for (int j = 0; j < comtradeManager.DataFile.BinaryData.Count; j++)
                    {
                        var binanry = new BinaryContent(comtradeManager.DataFile.BinaryData[j],
                              comtradeManager.ConfigFile.RowChannelNumType.AnalogChannelCount,
                            comtradeManager.ConfigFile.RowChannelNumType.DigitalChannelCount);
                        var assii = new ASCIIContent(binanry);
                        Console.Write(assii.RowToString());
                    }


                    Console.WriteLine("TestComtradeManager-TestEnd");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);

                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                }
            }



        }


        static void TestBinary()
        {
            Console.WriteLine("TestBinary-TestStart");
            byte[] data = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 0xFC, 0xB5, 0x04, 0x64, 0x00, 0x1D, 0x00, 0x79, 0xFF, 0x3B, 0xFF, 0x00, 0x00 };
            // var row = new BinaryContent(data, 6, 6);

            string testStr = "2, 167, -943, 1231 , 94, 37, -137 , -275, 1, 1, 0, 0, 1, 1";
            var rowAssii = new ASCIIContent(testStr, 6, 6);
            var row = new BinaryContent(rowAssii);
            var rowdata = row.RowToByteArray();
            foreach (var m in rowdata)
            {
                Console.Write(m.ToString("X2") + " ");

            }
            Console.WriteLine();
            Console.WriteLine(row.SampleNum.ToString());
            Console.WriteLine(row.TimeStamp.ToString());
            foreach (var m in row.AnalogChannelData)
            {
                Console.WriteLine(m);
            }
            foreach (var m in row.DigitalChannelData)
            {
                Console.WriteLine(m);
            }

            Console.WriteLine();
            byte[] data2 = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 0xFC, 0xB5, 0x04, 0x64, 0x00, 0x1D, 0x00, 0x79, 0xFF, 0x3B, 0xFF, 0x00, 0x00 };

            row.ByteToRow(data2, 6, 6);
            rowdata = row.RowToByteArray();
            foreach (var m in rowdata)
            {
                Console.Write(m.ToString("X2") + " ");

            }
            Console.WriteLine();
            Console.WriteLine("TestBinary-TestEnd");



            Console.ReadLine();

        }

        static void TestASCII()
        {
            Console.WriteLine("TestASCIIContent-TestStart");
            string testStr = "2, 167, -943, 1231 , 94, 37, -137 , -275, 0, 0, 0, 0, 0, 0";
            // var row = new ASCIIContent(testStr,6,6);
            byte[] data = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1E, 0xFC, 0xB5, 0x04, 0x64, 0x00, 0x1D, 0x00, 0x79, 0xFF, 0x3B, 0xFF, 0x00, 0x00 };
            var rowBinary = new BinaryContent(data, 6, 6);

            var row = new ASCIIContent(rowBinary);

            Console.Write(row.RowToString());
            testStr = "2, 45, -943, 2322 , 94, 37, -137 , -275, 1, 1, 0, 0, 0, 0"; ;

            row.StringToRow(testStr, 6, 6);

            Console.Write(row.RowToString());
            Console.WriteLine("TestASCIIContent-TestEnd");

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Input:");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                    else
                    {
                        row.StringToRow(instr, 6, 6);

                        Console.WriteLine(row.RowToString());

                        Console.WriteLine("TestEnd");
                        Console.WriteLine();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void TestComtradeConfig()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("TestComtrade-TestStart");
                    string str;
                    using (var file = File.OpenRead(@"file\cfg.cfg"))
                    {
                        StreamReader stream = new StreamReader(file);

                        str = stream.ReadToEnd();

                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(str);
                    Console.WriteLine();
                    Console.WriteLine();
                    ComtradeConfigFile comtrade = new ComtradeConfigFile();
                    comtrade.FileToRowMessage(str);

                    string path = @"file\" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".cfg";
                    using (var file = File.OpenWrite(path))
                    {
                        StreamWriter stream = new StreamWriter(file);
                        string[] strcollect;
                        comtrade.MakeConfigFile(out strcollect);
                        foreach (var m in strcollect)
                        {
                            stream.Write(m);
                        }
                        stream.Flush();

                    }

                    Console.WriteLine("TestComtrade-TestEnd");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                }
            }
        }

        static void TestDataStamp()
        {
            Console.WriteLine("TesttDataStamp-TestStart");
            string testStr = "11/07/1995,17:38:26.663700";
            var row = new DateStamp(testStr);

            Console.Write(row.RowToString());
            testStr = "12/02/2017,11:56:01.234789"; ;

            row.StringToRow(testStr);

            Console.Write(row.RowToString());
            Console.WriteLine("TesttDataStamp-TestEnd");

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Input:");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                    else
                    {
                        row.StringToRow(instr);

                        Console.WriteLine("DayOfMonth:" + row.DayOfMonth);
                        Console.WriteLine("Month:" + row.Month);
                        Console.WriteLine("Year:" + row.Year);
                        Console.WriteLine("Hour:" + row.Hour);
                        Console.WriteLine("Minute:" + row.Minute);
                        Console.WriteLine("Second:" + row.Second);


                        Console.WriteLine("TestEnd");
                        Console.WriteLine();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void TestDigitalChannelInformation()
        {
            Console.WriteLine("TestDigitalChannelInformation-TestStart");
            string testStr = "1,Popular Va-g,,,0";
            var row = new DigitalChannelInformation(testStr);

            Console.Write(row.RowToString());
            testStr = "3,Ia over,,,0";

            row.StringToRow(testStr);

            Console.Write(row.RowToString());
            Console.WriteLine("TestAnalogChannelInformation-TestEnd");

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Input:");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                    else
                    {
                        row.StringToRow(instr);

                        Console.WriteLine("ChannelIndex:" + row.ChannelIndex);
                        Console.WriteLine("ChannelID:" + row.ChannelID);
                        Console.WriteLine("MonitorComponent:" + row.MonitorComponent);
                        Console.WriteLine("ChannelPhaseID:" + row.ChannelPhaseID);
                        Console.WriteLine("StatusNormal:" + row.StatusNormal);



                        Console.WriteLine("TestEnd");
                        Console.WriteLine();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void TestAnalogChannelInformation()
        {
            Console.WriteLine("TestAnalogChannelInformation-TestStart");
            string testStr = "1,Popular Va-g,,,kV,0.123345,0.00000000,0,-2048,2047,2000,1,p";
            var row = new AnalogChannelInformation(testStr);

            Console.Write(row.RowToString());
            testStr = "3,Popular Ia-g,,,kV,0.123345,1.00000000,0,-88888,2047,2000,1,p";

            row.StringToRow(testStr);

            Console.Write(row.RowToString());
            Console.WriteLine("TestAnalogChannelInformation-TestEnd");

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Input:");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                    else
                    {
                        row.StringToRow(instr);

                        Console.WriteLine("ChannelIndex:" + row.ChannelIndex);
                        Console.WriteLine("ChannelID:" + row.ChannelID);
                        Console.WriteLine("MonitorComponent:" + row.MonitorComponent);
                        Console.WriteLine("ChannelPhaseID:" + row.ChannelPhaseID);
                        Console.WriteLine("ChannelUnit:" + row.ChannelUnit);
                        Console.WriteLine("ChannelGain:" + row.ChannelGain);
                        Console.WriteLine("ChannelOffset:" + row.ChannelOffset);
                        Console.WriteLine("Skewing:" + row.Skewing);
                        Console.WriteLine("Min:" + row.Min);
                        Console.WriteLine("Max:" + row.Max);
                        Console.WriteLine("Primary:" + row.Primary);
                        Console.WriteLine("Primary:" + row.Secondary);
                        Console.WriteLine("PS:" + row.PS);


                        Console.WriteLine("TestEnd");
                        Console.WriteLine();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void TestChannelNumType()
        {
            Console.WriteLine("TestChannelNumType-TestStart");
            var row = new ChannelNumType(2, 1, 1);
            Console.Write(row.RowToString());
            string testStr = "3,2A,1D";

            row.StringToRow(testStr);

            Console.Write(row.RowToString());
            Console.WriteLine("TestChannelNumType-TestEnd");

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Input:");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                    else
                    {
                        row.StringToRow(instr);
                        Console.WriteLine("ChannelTotalNum:" + row.ChannelTotalNum);
                        Console.WriteLine("AnalogChannelNum:" + row.AnalogChannelNum);
                        Console.WriteLine("DigitalChannelNum:" + row.DigitalChannelNum);

                        Console.Write(row.RowToString());

                        Console.WriteLine("TestEnd");
                        Console.WriteLine();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void TestStationRev()
        {
            Console.WriteLine("TestStart");
            var row = new StationRev("中国", "sd", "1991");
            Console.Write(row.RowToString());
            string testStr = "Condie,518,1997";

            row.StringToRow(testStr);

            Console.Write(row.RowToString());
            Console.WriteLine("TestEnd");

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Input:");
                    var instr = Console.ReadLine();
                    if (instr == "q")
                    {
                        return;
                    }
                    else
                    {
                        row.StringToRow(instr);
                        Console.WriteLine("StationName:" + row.StationName);
                        Console.WriteLine("RecordDeviceID:" + row.RecordDeviceID);
                        Console.WriteLine("VersionYear:" + row.VersionYear);

                        Console.Write(row.RowToString());

                        Console.WriteLine("TestEnd");
                        Console.WriteLine();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IBCS.BF.Key;
using Newtonsoft.Json.Converters;
using System.Reflection;
using IBCS.Math;
using IBCS.EC;

namespace IBCSApp.Converters
{
    public class KeyConverter : CustomCreationConverter<BFUserPrivateKey>
    {
        public override BFUserPrivateKey Create(Type objectType)
        {
            return new BFUserPrivateKey();
        }


        private Point ReadPoint(JsonReader reader)
        {
            BigInt[] coords = new BigInt[2];
            for (int i = 0; i < 2; i++)
            {
                coords[i] = ReadBigInt(reader);
                reader.Read();
            }
            return new Point(coords[0], coords[1]);
        }

        private BigInt ReadBigInt(JsonReader reader)
        {
            while (reader.TokenType != JsonToken.String)
            {
                reader.Read();
            }
            return new BigInt((string)reader.Value, 16);
        }

        private EllipticCurve ReadCurve(JsonReader reader)
        {
            Fp field = null;
            EllipticCurve curve = null;
            BigInt A = null;
            BigInt B = null;
            bool rA = false;
            bool rB = false;
            bool rField = false;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string rValue = reader.Value.ToString().ToLower();
                    if (rValue == "field")
                    {
                        field = new Fp(ReadBigInt(reader));
                        rField = true;
                    }
                    else if (rValue == "a")
                    {
                        A = ReadBigInt(reader);
                        rA = true;
                    }
                    else if (rValue == "b")
                    {
                        B = ReadBigInt(reader);
                        rB = true;
                    }
                }
                if (rA && rB && rField)
                {
                    return new EllipticCurve(field, A, B);
                }
            }
            return curve;
        }

        private TatePairing ReadPairing(JsonReader reader)
        {
            EllipticCurve curve = null;
            TatePairing e = null;
            BigInt cofactor = null;
            BigInt order = null;
            bool rCurve = false;
            bool rCofactor = false;
            bool rOrder = false;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string rValue = reader.Value.ToString().ToLower();
                    if (rValue == "curve")
                    {
                        curve = ReadCurve(reader);
                        rCurve = true;
                    }
                    else if (rValue == "cofactor")
                    {
                        cofactor = ReadBigInt(reader);
                        rCofactor = true;
                    }
                    else if (rValue == "grouporder") {
                        order = ReadBigInt(reader);   
                        rOrder = true;
                    }
                }
                if (rCofactor && rCurve && rOrder)
                {
                    return new TatePairing(curve, order, cofactor);
                }
            }
            return e;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var mappedObj = new BFUserPrivateKey();
            //get an array of the object's props so I can check if the JSON prop s/b mapped to it
            var objProps = objectType.GetProperties().Select(p => p.Name.ToLower()).ToArray();
            Point pPrivate, pPoint, pPub;
            pPub = pPrivate = pPoint = null;
            TatePairing e = null; ;
            BFMasterPublicKey pKey;
            bool rPPrivate, rPairing, rP, rPPub, rKey, rParam;
            rPPub = rPairing = rP = rPPrivate = rKey = rParam = false;

            //loop through my JSON string
            while (reader.Read())
            {
                //if I'm at a property...
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    //convert the property to lower case
                    string readerValue = reader.Value.ToString().ToLower();
                    if (readerValue == "key") { //We are expecting point with X and Y coordinates
                        pPrivate = ReadPoint(reader);
                        rPPrivate = true;
                        rKey = true;
                    }
                    else if (readerValue == "param") //We are expecting a pairing a 2 points
                    {
                        while (reader.Read() && rParam == false)
                        {
                            if (reader.TokenType == JsonToken.PropertyName)
                            {
                                string rValue = reader.Value.ToString().ToLower();
                                if (rValue == "pairing")
                                {
                                    e = ReadPairing(reader);
                                    rPairing = true;
                                }
                                else if (rValue == "p")
                                {
                                    pPoint = ReadPoint(reader);
                                    rP = true;
                                }
                                else if (rValue == "ppub")
                                {
                                    pPub = ReadPoint(reader);
                                    rPPub = true;
                                }
                            }
                            if (rP && rPPub && rPairing)
                            {
                                rParam = true;
                            }
                        }
                    }
                }
                if (rParam && rKey)
                {
                    pKey = new BFMasterPublicKey(e, pPoint, pPub);
                    while (reader.Read())
                    {
                        reader.Read();
                    }
                    return new BFUserPrivateKey(pPrivate, pKey);
                }
            }
            return mappedObj;
        }
    }
}

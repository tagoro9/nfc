using System;
using IBCS.Interfaces;
using IBCS.Math;

namespace IBCS.EC
{
    public class Predefined
    {
        public static TatePairing ssTate()
        {
            BigInt p = new BigInt("8BA2A5229BD9C57CFC8ACEC76DFDBF3E3E1952C6B3193ECF5C571FB502FC5DF410F9267E9F2A605BB0F76F52A79E8043BF4AF0EF2E9FA78B0F1E2CDFC4E8549B", 16);
            Field field = new Fp(p);

            BigInt orderOfGroup = new BigInt("8000000000000000000000000000000000020001", 16);

            BigInt cof = new BigInt("117454A4537B38AF9F9159D8EDBFB7E7C7C2E48760E930A461D5F451F9D9210DC70095F4B241FF57F1BB0549C", 16);
            //curve 
            //y^2=x^3+x
            FieldElement a = field.GetOne();
            FieldElement b = field.GetZero();
            EllipticCurve ec = new EllipticCurve(field, a, b);
            return new TatePairing(ec, orderOfGroup, cof);
        }

        public static TatePairing ssTate2()
        {
            BigInt p = new BigInt("8780710799663312522437781984754049815806883199414208211028653399266475630880222957078625179422662221423155858769582317459277713367317481324925129998224791", 10);
            Field field = new Fp(p);

            BigInt orderOfGroup = new BigInt("730750818665451621361119245571504901405976559617", 10);

            BigInt cof = new BigInt("12016012264891146079388821366740534204802954401251311822919615131047207289359704531102844802183906537786776", 10);

            //curve 
            //y^2=x^3+x

            FieldElement a4 = field.GetOne();
            FieldElement a6 = field.GetZero();
            EllipticCurve ec = new EllipticCurve(field, field.GetZero(), field.GetZero(), field.GetZero(), a4, a6);
            return new TatePairing(ec, orderOfGroup, cof);
        }

        public static TatePairing nssTate()
        {
            BigInt p = new BigInt("8D5006492B424C09D2FEBE717EE382A57EBE3A352FC383E1AC79F21DDB43706CFB192333A7E9CF644636332E83D90A1E56EFBAE8715AA07883483F8267E80ED3", 16);
            Field field = new Fp(p);
            //order =2^159+2^17+1		
            BigInt orderOfGroup = new BigInt("8000000000000000000000000000000000020001", 16);
            //BigInt orderOfE= new BigInt("11711338024714009669995700965425239711927177698599625717955894184681899877662611539569996945969293708404400344208273812850399351303651875378098503534075638");

            //co-factor
            BigInt cof = new BigInt("11AA00C9256849813A5FD7CE2FDC7054AFD7809E7F7FD948C4B9C1C1E76FFEFF4ECAB83C950112DECB41D6EDA", 16);
            FieldElement a4 = field.Negate(BigInt.ValueOf(3));
            FieldElement a6 = new BigInt("609993837367998001C95B87A6BA872135E26906DB4C192D6E038486177A3EDF6C50B9BB20DF881F2BD05842F598F3E037B362DBF89F0A62E5871D41D951BF8E", 16);
            EllipticCurve ec = new EllipticCurve(field, field.GetZero(), field.GetZero(), field.GetZero(), a4, a6);
            return new TatePairing(ec, orderOfGroup, cof);
        }
    }
}

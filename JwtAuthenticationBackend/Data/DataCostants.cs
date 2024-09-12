using System;

namespace JwtAuthenticationBackend.Data;

public class DataCostants
{
    //fields
    private static string _Admin="Administrator";
    private static string _Admin_Normalize="ADMINISTRATOR";

    private static string _User ="User";
    private static string _User_Normalize ="USER";

    // private static string _Seller="Seller";
    // private static string _Seller_Normalize="SELLER";

    // private static string _Customer="Customer";
    // private static string _Customer_Normalize="CUSTOMER";


    //properties
    public static string Admin { get{return _Admin;} }
    public static string Admin_Normalize { get{return _Admin_Normalize;} }
    public static string User { get{return _User;}}
    public static string User_Normalize { get{return _User_Normalize;}}
    // public static string customer { get{return _Customer;} }
    // public static string Customer_Normalize { get{return _Customer_Normalize;}}
    // public static string Seller { get{return _Seller;}}
    // public static string Seller_Normalize { get{return _Seller_Normalize;}}
}

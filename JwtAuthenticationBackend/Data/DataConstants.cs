using System;
using System.Reflection.Metadata;

namespace JwtAuthenticationBackend.Data;

public static class DataConstants
{
    //fields
    private const string _Admin="Administrator";
    private static string _Admin_Normalize="ADMINISTRATOR";

    private const string _User ="User";
    private static string _User_Normalize ="USER";

    private static int _RefreshTokenExpirationTime=1;
    private const int _TokenExpirationTime = 2;


    //properties
    public static string Admin { get{return _Admin;} }
    public static string Admin_Normalize { get{return _Admin_Normalize;} }
    public static string User { get{return _User;}}
    public static string User_Normalize { get{return _User_Normalize;}}


    public const string Admin_Role=_Admin;
    public const string User_Role = _User;

    //refresh token expiration time in day
    public static int RefreshTokenExpiration { get{return _RefreshTokenExpirationTime;} }
    public static int TokenExpirationTime { get{return _TokenExpirationTime;} }
}

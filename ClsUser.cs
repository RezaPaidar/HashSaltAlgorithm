public class ClsUser : IUser
{
    /*You can use free sha1 tools from here https://wee.tools/sha1-hash-key-generator/ 
    eg: I convert 123 to sha1 cryptography as the result:
    */
    private const string key = "40bd001563085fc35165329ea1ff5c5ecbdbbeef";
    
    public long Check_User_Password_Exist(string username, string password)
    {
        try
        {
            using (var db = new DbModel())
            {
                string encrypt = ClsEncryption.Encrypt(password, key);
                var user = db.Users.FirstOrDefault(x => x.PhoneNumber == username || x.uEmail == username);
                if (user != null && user.userPassword == encrypt)
                    return user.userName;
                return -1;
            }
        }
        catch (SqlException ex)
        {
            HasError.msg = clsError.getSQLError(ex.HResult);
            HasError.Error = ex.HResult;
            return ex.HResult * (-1);
        }
    }
    
    
    
}

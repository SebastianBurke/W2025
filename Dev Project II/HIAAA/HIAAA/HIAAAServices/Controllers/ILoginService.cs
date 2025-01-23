using System.Runtime.Serialization;
using CoreWCF;

[ServiceContract(Namespace = "https://cegep-heritage.qc.ca")]
public interface ILoginService
{
    [OperationContract(Action = "https://cegep-heritage.qc.ca/Authenticate")]
    AuthenticateResponse Authenticate(Authenticate request);

    [OperationContract(Action = "https://cegep-heritage.qc.ca/Authorize")]
    AuthorizeResponse Authorize(Authorize request);
}

[MessageContract]
public class Authenticate
{
    [MessageBodyMember(Namespace = "https://cegep-heritage.qc.ca", Order = 0)]
    public string Username { get; set; }
    
    [MessageBodyMember(Namespace = "https://cegep-heritage.qc.ca", Order = 1)]
    public string Password { get; set; }
}


[MessageContract]
public class AuthenticateResponse
{
    [MessageBodyMember]
    public bool AuthenticateResult { get; set; }
}

[MessageContract]
public class Authorize
{
    [MessageBodyMember(Order = 0)]
    public string Username { get; set; }

    [MessageBodyMember(Order = 1)]
    public string AppCode { get; set; }
}

[MessageContract]
public class AuthorizeResponse
{
    [MessageBodyMember]
    public UserBLL AuthorizeResult { get; set; }
}

[DataContract(Namespace = "https://cegep-heritage.qc.ca")]
public class UserBLL
{
    [DataMember]
    public int? StudentId { get; set; }

    [DataMember]
    public int? EmployeeId { get; set; }

    [DataMember]
    public long UserId { get; set; }

    [DataMember]
    public string? FirstName { get; set; }

    [DataMember]
    public string? LastName { get; set; }

    [DataMember]
    public string? Username { get; set; }

    [DataMember]
    public string? ErrorMessage { get; set; }

    [DataMember]
    public bool HasError { get; set; }

    [DataMember]
    public RoleBLL[] RoleList { get; set; }
}

[DataContract(Namespace = "https://cegep-heritage.qc.ca")]
public class RoleBLL
{
    [DataMember]
    public long RoleId { get; set; }

    [DataMember]
    public string? Code { get; set; }

    [DataMember]
    public string? Description { get; set; }
}

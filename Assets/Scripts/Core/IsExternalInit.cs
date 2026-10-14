/// <summary>
/// Unity uses an old .NET version which does not provide this type, which must be defined
/// in order to use init-only setters and in turn records. To get around this, we can define
/// our own type with this exact name and no body.
/// </summary>

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit {}
}
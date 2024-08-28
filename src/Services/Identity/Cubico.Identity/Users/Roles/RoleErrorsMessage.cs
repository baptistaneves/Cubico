namespace Cubico.Identity.Users.Roles;

public class RoleErrorsMessage
{
    public const string IdIsNotValid = "The provided ID is not valid";
    public const string RoleNameAlreadyExists = "This role already exists";
    public const string RoleNameIsRequired = "Role Name is required";
    public const string RoleNameMinimuLength = "Role Name must have at least 3 characters";
    public const string RoleNotFound = "Role was not found";
    public const string RoleCanNotBeRemoved = "This role is associated with users, cannot be removed";
}
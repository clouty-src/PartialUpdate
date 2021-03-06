# PartialUpdate
PartialUpdate is a library for ASP.Net Web API which helps you to implement partial resource update using http request with json body, that contains those fields that need to be updated.

## Usage

```csharp
// Update command definition
public class UserUpdateCommand
{
    [JsonProperty("name")]
    public Assignable<String> Name { get; set; }

    [JsonProperty("about")]
    public Assignable<String> About { get; set; }
}

// Update action
[Route("users/{userID}"), HttpPatch]
public IHttpActionResult UpdateUser(
    Guid userID,
    [PartialUpdate] UserUpdateCommand userUpdateCommand)
{
    var user = UserRepository.GetByID(userID);

    userUpdateCommand.Name.Apply(n => user.Name = n);
    userUpdateCommand.About.Apply(a => user.About = a);

    UserRepository.Save(user);

    return Ok();
}
```

## Nuget

[Partial Update Package](https://www.nuget.org/packages/Clouty.PartialUpdate)

## Collaboration

If you use this library and know how to make it better - create [issue](https://github.com/clouty-src/PartialUpdate/issues/new) or fork, improve and make pull request!

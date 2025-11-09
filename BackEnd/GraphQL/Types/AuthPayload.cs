namespace itsc_dotnet_practice.GraphQL.Types;

public record AuthPayload(string Token, UserPayload User);

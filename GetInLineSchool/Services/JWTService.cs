namespace GetInLineSchool.Services;
using GetInLineSchool.Models;
using System.Security.Claims;
using DotNetEnv;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

public class JWTService
{
    public static string CreateToken(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        int id = 0, role = 0;

        switch(user)
        {
            case Student s:
                id = s.IDStudent;
                role = s.Role;
                break;
            case Teacher t:
                id = t.IDTeacher;
                role = t.Role;
                break;
            case Admin a:
                id = a.IDAdmin;
                role = a.Role;
                break;
        }

        Claim[] claims = new Claim[]
        {
            new Claim("id",id.ToString()),
            new Claim("role",role.ToString())
        };

        Env.Load();

        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

        if (string.IsNullOrEmpty(jwtSecret))
            throw new Exception("JWT_SECRET environment variable is not set");       

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "GetInLineSchool",
            audience: "front-end",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(45),
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

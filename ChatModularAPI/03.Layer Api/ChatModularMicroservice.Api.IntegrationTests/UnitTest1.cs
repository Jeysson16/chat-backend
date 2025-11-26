using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using ChatModularMicroservice.Entities.DTOs;
using System.Text.Json;

namespace ChatModularMicroservice.Api.IntegrationTests;

public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_Returns_Unauthorized_For_Invalid_Credentials()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginDto = new LoginDto
        {
            UserCode = "usuario_inexistente",
            Password = "clave_incorrecta",
            RememberMe = false
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_Returns_Ok_For_Demo_User()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginDto = new LoginDto
        {
            UserCode = "demo",
            Password = "demo",
            RememberMe = false
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

        // Assert basic behavior
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("fake-jwt");
    }

    [Fact]
    public async Task Login_Returns_BadRequest_For_Invalid_Model()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginDto = new LoginDto
        {
            UserCode = "", // Código vacío
            Password = "",  // Contraseña vacía
            RememberMe = false
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Auth_Status_Returns_Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/auth/status");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotBeNullOrEmpty();
        body.Should().Contain("Active");
    }

    [Fact]
    public async Task Login_With_RememberMe_True_Works_Correctly()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginDto = new LoginDto
        {
            UserCode = "demo",
            Password = "demo",
            RememberMe = true // Probar con RememberMe activado
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        
        // Verificar que la respuesta es JSON válido
        body.Should().NotBeNullOrEmpty();
        
        // Opcionalmente, podemos deserializar para verificar la estructura
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("isSuccess").GetBoolean().Should().BeTrue();
    }
}

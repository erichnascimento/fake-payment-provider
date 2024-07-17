using System.Text.Json.Serialization;
using WebApiApplication.HttpRestApi.Handles.Base;
using WebApiApplication.HttpRestApi.Handles.CreatePayment;

namespace WebApiApplication.HttpRestApi;

/// <summary>
/// This class is used to configure the JSON serialization options for the application.
/// It is used in the Program.cs file to configure the JSON serialization options for the application.
/// Add the [JsonSerializable] attribute to the classes that you want to serialize/deserialize.
/// The classes that are marked with the [JsonSerializable] attribute will be serialized/deserialized
/// using the JSON serialization options configured in this class.
/// </summary>

// Add the [JsonSerializable] attribute to the classes that you want to serialize/deserialize.

// Payment requests and responses
[JsonSerializable(typeof(CreatePaymentRequest))]
[JsonSerializable(typeof(CreatePaymentResponse))]
[JsonSerializable(typeof(GetPaymentResponse))]

// Generic responses
[JsonSerializable(typeof(CreatedResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;
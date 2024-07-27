# Fake Payment Provider

[![.NET](https://github.com/erichnascimento/fake-payment-provider/actions/workflows/dotnet.yml/badge.svg)](https://github.com/erichnascimento/fake-payment-provider/actions/workflows/dotnet.yml)

This is a fake payment provider that simulates a payment provider. It is used for testing purposes.

## Table of Contents

- [Fake Payment Provider](#fake-payment-provider)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
  - [Usage](#usage)
  - [API](#api)
    - [POST /payment](#post-payment)
  - [Development](#development)
    - [Prerequisites](#prerequisites)
    - [Running the server](#running-the-server)
    - [Running the tests](#running-the-tests)
  - [Roadmap](#roadmap)
  - [License](#license)

## Installation
    
TODO

## Usage

TODO

## API

### POST /v1/payments

This endpoint simulates a payment request. 
It will return a 201 status code if the payment was successful, 
and a 400 status code if the payment failed.

#### Create a credit card payment

Request body:

```json
{
  "amount": 100.01,
  "currency": "BRL",
  "paymentMethod": "creditcard",
  "payer": {
    "name": "John Doe",
    "email": "johndoe@example.com",
    "cellPhoneNumber": "5511999887654",
    "document": {
      "type": "CPF",
      "number": "12345678909"
    },
    "address": {
      "street": "Rua dos Bobos",
      "number": "10",
      "complement": "Sala 1",
      "neighborhood": "Centro",
      "city": "São Paulo",
      "state": "SP",
      "country": "BR",
      "zipCode": "80100200"
    },
    "card": {
      "number": "4242424242424242",
      "holderName": "John Doe",
      "expiration": "12/2023",
      "cvc": "123",
      "store": true
    }
  },
  "paymentCode": "123456",
  "postbackUrl": "https://localhost:3000/postback?t=a4577&paymentCode=123456"
}
```

Response:

` 201 Created` 
```json
{
  "id": "e5a78634-6d25-43b0-8d4f-c1bc57496d4c",
  "paymentCode": "123456",
  "status": "confirmed",
  "authorizationCode": "123456",
  "cardToken": "f7b1b8b0-1b7b-4b6b-8b1b-0b1b7b4b6b8b"
}
```

#### Create a boleto payment

Request body:

```json
{
  "amount": 100.01,
  "currency": "BRL",
  "paymentMethod": "boleto",
  "dueDate": "2021-12-31",
  "withPix": true,
  "payer": {
    "name": "John Doe",
    "email": "johndoe@example.com",
    "cellPhoneNumber": "5511999887654",
    "document": {
      "type": "CPF",
      "number": "12345678909"
    },
    "address": {
      "street": "Rua dos Bobos",
      "number": "10",
      "complement": "Sala 1",
      "neighborhood": "Centro",
      "city": "São Paulo",
      "state": "SP",
      "country": "BR",
      "zipCode": "80100200"
    }
  },
  "paymentCode": "123456",
  "postbackUrl": "https://localhost:3000/postback?t=a4577&paymentCode=123456"
}
```

Response:

` 201 Created` 
```json
{
  "id": "e5a78634-6d25-43b0-8d4f-c1bc57496d4c",
  "paymentCode": "123456",
  "status": "pending",
  "boleto": {
    "number": "34191.09008 63521.510047 91020.150008 5 12345678901234",
    "barcode": "34191510047910201500085012345678901234",
    "dueDate": "2021-12-31"
  },
  "pix": {
    "copyPaste": "00020126580014br.gov.bcb.pix0136123e4567-e12b-12d1-a456-426655440000 5204000053039865802BR5913Fulano de Tal6008BRASILIA62070503***63041D3D",
    "qrCodeBase64": "iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAYAAACtWK6eAAAB...",
    "dueDate": "2021-12-31"
  }
}
```

### POST /v1/payments/{id}/confirm

#### Confirm a boleto payment

Request body:

```json
{
    "paymentMethod": "boleto",
    "paidAmount": 100.01,
    "paidOn": "2021-12-31"
}
```

Response:

` 200 OK` 
```json
{
  "id": "e5a78634-6d25-43b0-8d4f-c1bc57496d4c",
  "paymentCode": "123456",
  "status": "confirmed",
  "paidAmount": 100.01,
  "paidOn": "2021-12-31"
}
```

## Development

The architecture of this project is based on the [Clean Architecture principles](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

![](https://github.com/erichnascimento/fake-payment-provider/blob/main/assets/clean-arch-diagram.jpeg)

The project is divided into the following layers:

- **Domain**: Contains the business rules and entities of the application.
- **Infra**: Contains the concrete implementation of the interfaces defined in the Domain layer.
- **WebApiApplication**: Contains the web API and the presentation layer of the application.
- **Library**: Contains the shared code between the layers.

### Prerequisites

TODO

### Running the server

```bash
dotnet run
```

The server will start at `http://localhost:5246`.

You can test the API using IntelliJ HTTP Client file at `./http-client/fake-payment-provider.http`.

#### Create a boleto payment example:

```http
POST http://localhost:5246/v1/payments
Content-Type: application/json

{
  "paymentMethod": "boleto",
  "amount": 100,
  "currency": "BRL",
  "dueDate": "2024-08-31"
}
```

#### Response

```http
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8

{
  "id": "e5a78634-6d25-43b0-8d4f-c1bc57496d4c",
  "status": "pending",
  "boleto": {
    "number": "34191.09008 63521.510047 91020.150008 5 12345678901234",
    "barcode": "34191510047910201500085012345678901234",
    "dueDate": "2024-08-31"
  }
}
```

#### Server logs

```log
2024-07-20 01:13:37.478 INFO  Http Request:             request={PaymentMethod=boleto, Amount=100, Currency=BRL ... }
2024-07-20 01:13:37.480 INFO  Http Request: status=201  request={PaymentMethod=boleto, Amount=100, Currency=BRL}  response={Id=e5a78634-6d25-43b0-8d4f-c1bc57496d4c, Status=pending ... }
```

### Running the tests

```bash
dotnet test
```

## Roadmap

- [x] Implements the payment boleto creation :checkered_flag:
- [ ] Implements the payment boleto confirmation (in progress :hourglass_flowing_sand:)
- [ ] Implements the payment credit card creation
- [ ] Implements the payment credit card confirmation
- [ ] Implements the payment credit card cancelation
- [ ] Implements the payment credit card refund
- [ ] Implements error simulation
- [ ] Implements the payment status query
- [ ] Implements the payment status webhook

## License

This project is licensed under the MIT License.



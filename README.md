# Fake Payment Provider

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
  }
}
```

Response:

` 201 Created` 
```json
{
  "id": "e5a78634-6d25-43b0-8d4f-c1bc57496d4c",
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
  }
}
```

Response:

` 201 Created` 
```json
{
  "id": "e5a78634-6d25-43b0-8d4f-c1bc57496d4c",
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
  "status": "confirmed",
  "paidAmount": 100.01,
  "confirmationDate": "2021-12-31"
}
```

## Development

### Prerequisites

TODO

### Running the server

```bash
dotnet run
```

### Running the tests

```bash
dotnet test
```

## License

This project is licensed under the MIT License.



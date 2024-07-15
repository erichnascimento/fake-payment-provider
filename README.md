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

### POST /payment

This endpoint simulates a payment request. It will return a 200 status code if the payment was successful, and a 400 status code if the payment failed.

Request body:

```json
{
  "amount": 100,
  "currency": "USD",
  "card": {
    "number": "4242424242424242",
    "exp_month": 12,
    "exp_year": 2022,
    "cvc": "123"
  }
}
```

Response body:

```json
{
  "status": "success"
}
```

## Development

### Prerequisites

- Dotnet 8.0

### Running the server

```bash
dotnet run
```

### Running the tests

```bash
dotnet test
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.



# YetAnotherJsonFormatter

A simple JSON formatter website that provides an API for formatting JSON documents

Many websites which offer API's, will return data in JSON format. Often the JSON provided has white space compressed to reduce the size of the data transferred. This site gives you a quick and easy way to format the JSON so you can read it. 


## Backend API

Yet Another {JSON formatter} also provides an API that you can call directly to format the JSON in a more readable format. 

## API Reference

All API access is over HTTPS, and accessed from https://api.yajsonfmt.app. All data is sent and received as JSON. 

### Format JSON

`POST /json`

**Example**

```json
{"firstName":"John","lastName":"Smith","isAlive":true,"age":27,"address":{"streetAddress":"21 2nd Street","city":"New York","state":"NY","postalCode":"10021-3100"},"phoneNumbers":[{"type":"home","number":"212 555-1234"},{"type":"office","number":"646 555-4567"},{"type":"mobile","number":"123 456-7890"}],"children":[],"spouse":null} 
```

**Query Parameters**

|Name          | Type      | Description                                                                               | Default   |
|:-------------|:----------|:------------------------------------------------------------------------------------------|:----------|
| `indent`     | `int`     | How many indentChar to write for each level in the hierarchy when compact is set to false | `2`       |
| `indentChar` | `char`    | Which character to use for indenting when compact is set to false                         | `<space>` |
| `compact`    | `boolean` | Value indicating how JSON text output should be formatted.                                | `false`   |
| `validate`   | `boolean` | Validate that the JSON sent is valid.                                                     | `true`    |
| `quoteChar`  | `char`    | Which character to use to quote attribute values.                                         | `"`       |
| `quoteNames` | `boolean` | Value indicating whether object names will be surrounded with quotes                      | `true`    |

**Response**

```json
{
    "firstName": "John",
    "lastName": "Smith",
    "isAlive": true,
    "age": 27,
    "address": {
        "streetAddress": "21 2nd Street",
        "city": "New York",
        "state": "NY",
        "postalCode": "10021-3100"
    },
    "phoneNumbers": [
        {
            "type": "home",
            "number": "212 555-1234"
        },
        {
            "type": "office",
            "number": "646 555-4567"
        },
        {
            "type": "mobile",
            "number": "123 456-7890"
        }
    ],
    "children": [],
    "spouse": null
} 
```

### Validate JSON

`POST /json/validate`

**Valid JSON**

```json
{"firstName":"John","lastName":"Smith","isAlive":true,"age":27,"address":{"streetAddress":"21 2nd Street","city":"New York","state":"NY","postalCode":"10021-3100"},"phoneNumbers":[{"type":"home","number":"212 555-1234"},{"type":"office","number":"646 555-4567"},{"type":"mobile","number":"123 456-7890"}],"children":[],"spouse":null} 
```

**Response**

```json
{
    "success": true,
    "message": "Input is valid JSON"
} 
```

**Invalid JSON**

```json
{
  "firstName": "John",
  "lastName": "Smith",
  "isAlive": true,
  "age": 27,
  "address": {
    "streetAddress": "21 2nd Street",
    "city": "New York",
    "state": "NY",
    "postalCode": "10021-3100"
  },
  "phoneNumbers": [
    {
      "type": "home",
      "number": "212 555-1234"
    },
    {
      "type": "office",
      "number": "646 555-4567"
    },
    {
      "type": "mobile",
      "number": "123 456-7890"
    }
  , <-- Hint, hint: Missing closing array bracket
  "children": [],
  "spouse": null
}
```

**Response**

```json
{
    "success": false,
    "message": "Wah wah wah, input is not valid JSON.\nUnexpected character encountered while parsing value: <. Path 'phoneNumbers[2]', line 25, position 4."
}
```
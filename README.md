Simple Test (All payloads are in JSON format, don't forget to add content-type = application/json in the header):

1) Register an admin user with this payload
```
POST to https://(your-localhost)/api/Account/Register
{
  "Email": "test@mail.test",
  "Password": "SomeSecurePassword123!",
  "Roles":["Admin"]
}
```
2) The JWT will be returned
3) Register a vehicle with this payload
```
POST to https://(your-localhost)/api/Vehicles
Headers:
Name: Authorization
Value: Bearer (JWT)
{
  "Name": "Supra"
}
```
4) Add a location with this payload
POST to https://(your-localhost)/api/Locations
Headers
Name: Authorization
Value: Bearer (JWT)
```
{
  "Latitude": 13.7760950,
  "Longitude": 100.5071776
}
```
5) Get the current position with this url
```
GET https://(your-localhost)/api/Locations/(vehicle-id-that-was-registered)
Headers
Name: Authorization
Value: Bearer (JWT)
```
6) Add more locations if needed
7) Get the position during the certain time with this url
```
GET https://(your-localhost)/api/Locations/(vehicle-id-that-was-registered)/Date?from=(xxx)&to=(yyy)
Headers
Name: Authorization
Value: Bearer (JWT)
```

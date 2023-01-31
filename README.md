# JumpServer
Bypass http requests to other sites

## Reuirement
.Net 6 SDK

## How to run
1. Build project by your way (ex: VS2022, MSBuild)
2. Run JumpServer

## How to use
1. start build an http request
2. input the target url in header with the key "url"
3. (optional) add a custom header to headers and add keys that combine with "," to header with the key "accepted-headers"
4. the bypass http method depends on your request method

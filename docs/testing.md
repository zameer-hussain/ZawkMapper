# Testing ZawkMapper

This project uses several testing levels.

## Smoke testing

Smoke tests answer one question: is this build alive?

They check basic mapping, projection, nested mapping, named projection, and runtime conversion.

## Unit-style testing

Unit-style tests check individual features:

- `MapFieldStrict`
- `MapField` runtime conversion
- enum conversion
- duplicate unnamed map detection
- named projection
- nested child projection
- collection-to-single child projection

## Performance testing

Performance lab records important numbers:

- configuration build time
- manual map baseline
- runtime map single object cold
- runtime map single object warm
- runtime map 10k warm
- runtime map 100k warm
- first projection build and run
- cached projection reuse
- nested projection 10k

## Recommended order

```text
1. restore package
2. build package
3. run smoke tests
4. run unit-style tests
5. run performance lab
6. run MVC sample
7. pack NuGet package
8. publish prerelease or stable version
```

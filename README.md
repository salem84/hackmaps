# Map your Issues

This repository contains a Github Action workflow triggered on new issues to create a World map of issues location.

## How it works
It is composed of three components:

- A Github workflow triggered by issue events and powered by free and open-source Github actions
- A simple Azure Function to receive event and write to Azure CosmosDb
- A PowerBI Report to show finally issues on world map!


### Workflow pipeline
Yaml pipeline uses two opensource Actions:
- `octokit/graphql-action`: to query about user location info using Github GraphQL API
- `satak/webrequest-action`: to post JSON data to Azure Function
  

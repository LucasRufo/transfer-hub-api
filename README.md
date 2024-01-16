# Money Rest API

Requirements:

- It should be possible to register a participant ✅
- It should be possible to create a new deposit for a participant ✅
- It should be possible to transfer money from one participant to another ✅
  - It shouldn't be possible to transfer if the participant does not have enough money ✅
  - It shouldn't be possible to transfer if one of the participants does not exist ✅
- It should be possible to get a statement with all transactions ❌

Tech Requirements:

- Needs to be a REST API 
- Needs to be able to run on Kubernetes
- Needs a CI pipeline to run the build and tests
- Needs to be easy to setup the dev environment

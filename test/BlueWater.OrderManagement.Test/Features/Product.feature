Feature: Test Product Web API
	Test CRUD functions porvided by Pruduct Web API

@ProductTest
Scenario: Successfully Create a product
	Given I have a new product name notebook unit price 99
	When I create the product in Bluewater systme
	Then The result should be 201.

	When I check the created product
	Then The result should be 200.

	When I update the unit price to 100
	Then The result should be 200.

	When I check the updated product
	Then The result should be 200.

	When I delete the test product
	Then The result should be 200.

Scenario: Failed Create a product with invalid unitPrice
	Given I have a new product name pen unit price -1
	When I create the product in Bluewater systme
	Then The result should be 400.
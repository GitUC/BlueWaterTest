Feature: Product
	Test CRUD functions porvided by Pruduct Web API

@createProduct
Scenario: Creeate a product
	Given I have a new product name notebook unit price 99
	When I create the product in Bluewater systme
	Then the result should be 201

	When I check the created product
	Then the result should be 200
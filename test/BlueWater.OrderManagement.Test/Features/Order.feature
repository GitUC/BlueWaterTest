Feature: Test Order
	test functions provided by Order Web API 

@HangfireTest
Scenario: Create an order 
Given Payload as JSON
"""
{
  "userAccount": "test@example.com",
  "orderDetails": [
    {
      "productName": "notebook",
      "quatity": 10,
      "unitPrice": 2.5
    },
    {
      "productName": "pencil",
      "quatity": 5,
      "unitPrice": 1
    }
  ]
}
"""
When the Admin process the order
Then The result should be 200.
Given Sleep 5 seconds
When I get job status as Processing
Then The result should be 200.

Scenario: Schedule a task to call scheduleJob
Given Schedule datetime Payload as JSON
"""
{
  "hours": 0,
  "minutes": 1,
  "seconds": 0,
  "scheduleTime": ""
}
"""
When the Admin process the schedule Job
Then The result should be 200.
Given Sleep 5 seconds
When I get job status as Scheduled
Then The result should be 200.
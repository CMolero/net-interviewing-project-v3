# Coolblue Assessment
 
 Requirements 
 - .net core version 6
 - Newtonsoft.Json version >= 13
 - Swashbuckle.AspNetCore >= 6
 
 
 
There is an existing endpoint that, given the information about the product, 
calculates the total cost of insurance according to the rules below:
  - If the product sales price is less than € 500, no insurance required
  - If the product sales price=> € 500 but < € 2000, insurance cost is € 1000
  - If the product sales price=> € 2000, insurance cost is €2000
  - If the type of the product is a smartphone or a laptop, add € 500 more to the insurance cost.

Task 1 [BUGFIX]:
	I just refactored the section and fixed it, originally the if was in a way that wasn't optimal.

Task 2 [REFACTORING]:
	- Decoupled the services and removed the bussiness rule class
	- ref and out should be avoided
	- used .net core capabilities such httpclientfactory
	- changed the general logic to be asynchronous

Task 3 [FEATURE 1]:
	Now we want to calculate the insurance cost for an order and for this, we are going to provide all the products that are in a shopping cart.
	

  - added an Order model and reused the previous Product and Product type logic

Task 4 [FEATURE 2]:
	We want to change the logic around the insurance calculation. 
	We received a report from our business analysts that digital cameras are getting lost more than usual. 		
	Therefore, if an order has one or more digital cameras, add € 500 to the insured value of the order.
	
  - Added an extra insurance logic that includes a dictionary that can be modified to add or remove products that might require extra insurance 
	
Task 5 [FEATURE 3]:
As a part of this story we need to provide the administrators/back office staff with a new endpoint that will allow them to upload surcharge rates per product type. This surcharge will then  need to be added to the overall insurance value for the product type.

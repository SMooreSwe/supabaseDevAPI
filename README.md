# supabase Minimal Developer API
This is a minimal API using the C# Supabase Client. The project was built as a proof of concept for a mob-programming project and allows for operations on a selected Supabase Table, with the addition of the requisite Supabase Url and API key as env variables where noted.

## The Models

The table in question contained a model for the table items as:
- Id - int (primary key within the table)
- Name - String
- Email - String
- CreatedAt - DateTime

The progam also includes contracts for the DTOs based on requests and responses relating to the above model.

## The Routes

All routes are based on the /jobdescriptions endpoints.  The following operations are contained within the API:

- GET - /jobdescriptions/ - returns a list of all job descriptions.
- GET - /jobdescriptions/{id} - returns the job description identified.
- POST - /jobdescriptions - adds a job description based on teh request and returns the job description id generated by the table.
- PUT - /jobdescriptions/{id} - updates the job description identified.
- DELETE - /jobdescriptions/{id} - deletes the selected job description.

## Next Steps

Proposed next steps are to build the API as a webcontroller, to move the project to a more Modular Monolith architectural style.

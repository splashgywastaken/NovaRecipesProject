
## API Reference

Before starting using API you need to do next steps:

#### Step №1 Setup project files

Change this parameters in all `env` files (except `env.api`) to use some smtp server:

```
#EMAIL SENDER SETTINGS
EMAILSENDER_SMTPSERVER=smtp.yandex.ru
EMAILSENDER_PORT=465
EMAILSENDER_USESSL=true
EMAILSENDER_USERNAME=nickdur
EMAILSENDER_SENDERADDRESS=nickdur@yandex.ru
EMAILSENDER_SENDERNAME=Site Administrator
EMAILSENDER_PASSWORD=ikcqdwddsxzeqheq
```

Above is an example of how those fields should be written.  
After all of this you can launch application using

```
docker-compose build
docker-compose up
```

Now, after application is launched, open swagger to use (test) API  

Also, would like to say that there is commented lines in `docker-compose.yml` and `docker-compose-override.yml` for mailing scheduler, which is not working right now

#### Step №2 Register

Using this method you need to register a new account

```http
  POST /api/v0.1/accounts/
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `name(login)` | `string` | **Required**. Users login |
| `email` | `string` | **Required**. User's email |
| `password` | `string` | **Required**. User's password |

#### Step №2 Confirm email

Now, using this method you need to confirm your email

```http
  POST /api/v0.1/accounts/request-confirm-email
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `email` | `string` | **Required**. User's email |

After using this method you will recieve an email to your email that was passed to a method 

In that email there will be a mock link to confirming it, ignore link itself, just use the last part in it as an argument to next method:

```http
  POST /api/v0.1/accounts/confirm-email/{confirmationId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `confirmationId` | `int` | **Required**. An Id used to confirm email |

After using this method you will recieve an email about success of your email confirmation 

Now you can authorize in your account using *Authorize* button in swagger and you ready to go and try API's methods yourself

### API Methods

#### Recipes

There is a certain order in usage of those methods, firstly you need to add new recipe to DB using next method

```http
 POST /api/v0.1/recipes/user
```

After using it you can populate recipe with new data by using other ```POST``` methods  

#### Ingredients

In this API ingredients has a special way of how you should work with them  
If you want to add a new ingredient to recipe you can either:  
1. Search for some existing ingredient using corresponding ```GET``` method and that's ingredient id to add it to recipe with ``` POST /api/v0.1/recipe/{recipeId}/ingredients ``` method
2. Add a new ingredient to DB and then use it the same way as in first case

#### Subscriptions

To subscribe to a new email mailing you can use methods of next two controllers:  
1. **RecipesSubscription** - to subscribe to a new recipes from some other user
2. **RecipeCommentsSubscription** - to subscribe to a new comments in some recipe
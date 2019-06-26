<?php

// Start the project session
session_start();

// Database for user credentials
require_once "../../scripts/database/user-contents.php";

echo <<<_END
<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <meta http-equiv="X-UA-Compatible" content="ie=edge">
  <link rel="stylesheet" href="dashboard.css">
  <link rel="icon" href="../../resources/favicon.png">
  <title>MailSheild - Dashboard </title>
</head>

<body>
_END;

  require "./dashboard.php";

echo <<<_END
</body>

</html>
_END;

// Close the connection
$conn->close();

?>

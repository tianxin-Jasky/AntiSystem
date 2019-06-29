<?php
ini_set("display_errors", 0);
if (!isset($_SESSION["login_successful"])) {
  header("Location: ../user-login");
  exit();
}

echo <<<_END
<h1>User Dashboard</h1>
<form action="./" method="post" enctype='multipart/form-data'>

  <label for="content">Upload file</label>
  <input type="file" accept=".txt" name="content" required>

  <input type="submit" value="Submit">

</form>

<a class="btn center" href="../user-login">Logout</a>

<div id="content">
_END;

$user_email = $_SESSION["user_email"];
$user_timestamp = date("Y-m-d H:i:s");

if ($_FILES) {

  // Gets the file name and file content
  $user_filename = $_FILES["content"]["name"];
  move_uploaded_file($_FILES["content"]["tmp_name"], $user_filename);

  // Add the username and file to content table
  $user_data = "INSERT INTO $table_name (user_email, user_filename, time_created)
                VALUES ('$user_email', '$user_filename', '$user_timestamp')";
  $conn->query($user_data);

  // Refresh the current page
  header("Location: ./");
  exit();
}

// Show all the user files
$user_query = "SELECT user_filename, time_created FROM $table_name WHERE user_email='$user_email'";
$result = $conn->query($user_query);
$flag = -1;
while($row = $result->fetch_assoc()) {
  $user_filename = $_FILES["content"]["name"];
  $name = $row[user_filename];
  $flag = $flag + 1 ;
  exec("C:\Users\lenovo\AppData\Local\Programs\Python\Python37\python F:\wamp64\www\site\pages\user-dashboard\module1.py {$name}",$out);
	echo "<div class='content-block'>
       <h1>$row[user_filename]</h1>
       <h2>$row[time_created]</h2>
       <h2>$out[$flag]<h2>
   </div>";
    }
echo "</div>";
?>



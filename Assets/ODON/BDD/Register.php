<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$loginUser = trim($_POST["loginUser"]);
$loginPass = trim($_POST["loginPass"]);

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully <br>";

// Use prepared statement to avoid SQL injection
$sql = "SELECT UserID, UserName, UserPassword, UserMail FROM user WHERE UserName = ? OR UserMail = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $loginUser, $loginUser); // "ss" means two string parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    echo "Username or email already taken";
} else {
    // Check if the input is an email or username, and insert accordingly
    if (filter_var($loginUser, FILTER_VALIDATE_EMAIL)) {
        // Input is an email, insert into UserMail
        $sql2 = "INSERT INTO user (UserMail, UserPassword) VALUES (?, ?)";
    } else {
        // Input is a username, insert into UserName
        $sql2 = "INSERT INTO user (UserName, UserPassword) VALUES (?, ?)";
    }

    // Hash the password before inserting
    $hashedPassword = password_hash($loginPass, PASSWORD_DEFAULT); // Encrypt the password

    // Prepare the insert query
    $stmt2 = $conn->prepare($sql2);
    $stmt2->bind_param("ss", $loginUser, $hashedPassword); // Bind username/email and hashed password

    if ($stmt2->execute()) {
        echo "New record created successfully";
    } else {
        echo "Error: " . $sql2 . "<br>" . $conn->error;
    }

    // Close the statement
    $stmt2->close();
}

// Close the statement and connection
$stmt->close();
$conn->close();
?>

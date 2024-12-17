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
$sql = "SELECT UserID, UserName, UserPassword FROM user WHERE UserName = ? OR UserMail = ?";

$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $loginUser, $loginUser); // "ss" means two string parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Output data for each row
    while ($row = $result->fetch_assoc()) {
        // Verify the entered password against the stored hash
        if (password_verify($loginPass, $row["UserPassword"])) {
            echo "Login Success, UserID: " . $row["UserID"];
        } else {
            echo "Password incorrect";
        }
    }
} else {
    echo "Username does not exist";
}

$stmt->close(); // Close the statement
$conn->close(); // Close the connection
?>

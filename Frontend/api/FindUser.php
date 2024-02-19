<?php
$username = $_GET['username'];
include_once("../db.php");
include_once("../func.php");
UpdateScore($username); // security hazard, i love it
UpdateAccuracy($username); // security hazard, i love it
$sql = "SELECT * FROM users WHERE username = '$username'";
$result = $db->query($sql);
if($result->num_rows > 0)
{
    while($row = $result->fetch_assoc())
    {
        $userid = $row['userid'];
        $totalscore = $row['totalscore'];
        $accuracy = str_replace(".",",",$row['accuracy']);
        $PP = $row['PP'];
        $resp = "$userid|$accuracy|$totalscore|$PP";
        echo $resp;
    }
} else {
    echo "0";
}
?>
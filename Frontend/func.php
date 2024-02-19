<?php

function GenerateScore(
$replayId, $name, $score, $combo,
$count50, $count100, $count300, $countMiss, $countKatu, $countGeki,
$FC, $mods, $avatarID, $rank)
{
    $time = time();
    return "$replayId|$name|$score|$combo|$count50|$count100|$count300|$countMiss|$countKatu|$countGeki|$FC|$mods|$avatarID|$rank|0|$time|0\r\n";
}
function UpdateScore($username)
{
    include("db.php");
    $sql = "SELECT SUM(`score`) AS totalscore FROM `scores` WHERE username = '$username'";
    $result = $db->query($sql);
    $row = $result->fetch_assoc();
    $totalscore = (int)$row['totalscore'];
    $sql2 = "UPDATE `users` SET `totalscore`='$totalscore' WHERE username = '$username'";
    $db->query($sql2);
    $db->close();
    return $totalscore;
}


function UpdateAccuracy($username)
{
    include("db.php");
    $acc = GetAccuracy($username);
    $sql2 = "UPDATE `users` SET `accuracy`='$acc' WHERE username = '$username'";
    $db->query($sql2);
    return $acc;
}
function GetAccuracy($username)
{
    include("db.php");
    $sql = "SELECT * FROM scores WHERE username = ?";
    $stmt = $db->prepare($sql);
    $stmt->bind_param("s", $username);
    $result = $stmt->execute();
    $stmt->bind_result($replayId,$bmap,$user,$score,$combo,$fc,$mods,$c300,$c100,$c50,$cGeki,$cKatu,$cMiss,$time,$mode);

    $accuracy = 0;
    $row = 0;

    while ($stmt->fetch()) {
        if($completed = "2")
        {
            $accuracy += (float)((int)$c50 * 50 + (int)$c100 * 100 + (int)$c300 * 300) / (float)(((int)$c300 + (int)$c100 + (int)$c50 + (int)$cGeki + (int)$cKatu + (int)$cMiss) * 300);
            $row++;
        }
    }
    $stmt->close();
    if ($row > 0) {
        return $accuracy / $row;
    } else {
        return 1; 
    }

}
function DecryptScore($iv, $key,$score)
{
    include("vendor/autoload.php");
    $iv = base64_decode($iv);
    $score = base64_decode($score);
    return mcrypt_decrypt(MCRYPT_RIJNDAEL_256, $key, $score, MCRYPT_MODE_CBC, $iv);
}
function InsertScore($score)
{
    include_once("db.php");
    $score = explode(":", $score);
    $hash = trim($score[0]);
    $user = trim($score[1]);
    $combo = trim($score[10]);
    $fc = trim($score[11]);
    $mods = trim($score[13]);
    $c300 = trim($score[3]);
    $c100 = trim($score[4]);
    $c50 = trim($score[5]);
    $geki = trim($score[6]);
    $katu = trim($score[7]);
    $miss = trim($score[8]);
    $time = trim($score[16]);
    $mode = trim($score[15]);
    $rid = "0";

    $sql = "INSERT INTO `scores` 
            (`ReplayId`, `BeatmapHash`, `username`, `score`, `combo`, `fc`, `mods`, 
            `count300`, `count100`, `count50`, `countGeki`, `countKatu`, `countMiss`, 
            `time`, `mode`) 
            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
    $stmt = $db->prepare($sql);
    $stmt->bind_param("sssssssssssssss", $rid, $hash, $user, $score[9], $combo, $fc, $mods, 
                      $c300, $c100, $c50, $geki, $katu, $miss, $time, $mode);

    $stmt->execute();
    $stmt->close();
}

function DoScores($checksum)
{
    include_once("db.php");
    $sql = "WITH RankedScores AS (
        SELECT *,
            ROW_NUMBER() OVER (PARTITION BY username ORDER BY CAST(score AS SIGNED) DESC) AS row_num
        FROM scores
        WHERE BeatmapHash = ?
    )
    SELECT *
    FROM RankedScores
    WHERE row_num = 1
    ORDER BY CAST(score AS SIGNED) DESC";
    $stmt = $db->prepare($sql);
    $stmt->bind_param("s",$checksum);
    $stmt->execute();
    $stmt->bind_result($replayId,$bmap,$user,$score,$combo,$fc,$mods,$c300,$c100,$c50,$cGeki,$cKatu,$cMiss,$time,$mode, $row_num);

    $row = 1;
    while($stmt->fetch())
    {
        echo GenerateScore($replayId,$user,$score,$combo,$c50,$c100,$c300,$cMiss,$cKatu,$cGeki,$fc,$mods,1,$row);
        $row+1;
    }
    $stmt->close();
}
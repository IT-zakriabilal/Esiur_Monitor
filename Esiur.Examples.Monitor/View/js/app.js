

 async function connect() {

    await router.navigate("connecting");

    try {
        window.con = await wh.get("iip://localhost:8080");
        window.con.on("close", connect);
        await router.navigate("home");
        window.liveService = await con.get("sys/live");
        window.historyService = await con.get("sys/history");

        window.liveService.on("ProcessStarted", x => {
            var link = document.createElement("I-LINK");
            link.query = { pid: x.Id };
            link.link = "processes/process";
            link.innerHTML = `${new Date().toLocaleString('en-US')} Process <b>${x.Name}</b> started.`;
            document.getElementById("log").appendChild(link)
        });

        window.liveService.on("ProcessEnded", x => {
            var msg = document.createElement("div");
            msg.innerHTML = `${new Date().toLocaleString('en-US')} Process <b>${x.Name}</b> ended.`;
            document.getElementById("log").appendChild(msg)
        });

    } catch (ex) {
        alert(ex);
    }
}







function toToday() {
    today = new Date; var t = daysBetween(startOfWeek, endOfWeek); if (t > 8) var e = (t / 7 - 1) / 2;
    else e = 0; var a = getMonday(today); startOfWeek = new Date(a), endOfWeek = new Date(a), startOfWeek.setDate(startOfWeek.getDate() - 7 * e),
endOfWeek.setDate(endOfWeek.getDate() + 7 * (e + 1) - 1), gantt.config.start_date = startOfWeek, gantt.config.end_date = endOfWeek, today = new Date, refresh()
} function chronologicalToggle()
{
    chronological = !chronological, refresh(), chronological ? ($("#chrono").html("Trade&nbspview"), $("#collapse").html(" - ")) : ($("#chrono").html
    ("Gantt&nbspview"), collapse ? $("#collapse").html("Expand") : $("#collapse").html("Collapse"))
} function left() {
    startOfWeek.setDate(startOfWeek.getDate() - 7), endOfWeek.setDate(endOfWeek.getDate() - 7), gantt.config.start_date = startOfWeek,
    gantt.config.end_date = endOfWeek, refresh()
} function right() {
    startOfWeek.setDate(startOfWeek.getDate() + 7), endOfWeek.setDate(endOfWeek.getDate() + 7), gantt.config.start_date = startOfWeek,
    gantt.config.end_date = endOfWeek, refresh()
} function toggleIn() { daysBetween(startOfWeek, endOfWeek) >= 14 ? timeFrame(-7) : (zoom = !1, timeFrame(0)) } function toggleOut() { zoom = !0, timeFrame(7) }
function timeFrame(t) {
    if (date_range = t, startOfWeek.setDate(startOfWeek.getDate() - date_range), endOfWeek.setDate(endOfWeek.getDate() + date_range),
    daysBetween(startOfWeek, endOfWeek) > 13) {
        zoom = !1; var e = function (t) {
            var e = gantt.date.date_to_str("%d %M"),
         a = gantt.date.add(gantt.date.add(t, 1, "week"), -1, "day"); return e(t) + " - " + e(a)
        }; gantt.templates.date_scale = e, gantt.config.scale_unit = "week", gantt.config.step = 1, gantt.config.subscales = [{ unit: "day", step: 1, date: "%d" }]
    } else zoom = !0, gantt.templates.date_scale = null, gantt.config.scale_unit = "day", gantt.config.date_scale = "%M %d", gantt.config.step = 1;
    gantt.config.start_date = startOfWeek, gantt.config.end_date = endOfWeek, refresh()
} function mediaQuery() { } function getMonday(t) { t = new Date(t); var e = t.getDay(), a = t.getDate() - e + (0 == e ? -6 : 1); return new Date(t.setDate(a)) }
function format(t) {
    if ("open" == t) return "Not Specified"; var e = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"),
    a = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"),
 n = t.getDay(), r = t.getDate(), o = ""; o = 1 == r || 21 == r || 31 == r ? "st" : 2 == r || 22 == r ? "nd" : 3 == r || 23 == r ? "rd" : "th";
    var d = t.getMonth(), s = t.getFullYear(); return e[n] + " " + r + "<SUP>" + o + "</SUP> " + a[d] + " " + s
} function taskFired(t) {
    if (str = "", t.id >= count) return window.location.href = t.action, 0; var e = t.id - 1; chronological || (e -= trades.length);
    var a = "</p> <p><a class='primary hollow button small-six columns' href=" + tasks[e].action + ">Edit</a>", n = format(tasks[e].start),
 r = format(tasks[e].end); "qright" == t.both && (n = "No start date"), "qleft" == t.both && (r = "No end date"),
str = str + " <div class='small-12 columns'><h2 class='lead'>" + tasks[e].name + "</h2><p>" + trades[tasks[e].trade - 1].name +
"</p> </div><div class='small-12 columns'>" + tasks[e].content + "</div><div class='small-12 columns'><p><br /> <b>Start: </b> " + n +
" " + tasks[e].startTime + "</p><p> <b>End: </b> " + r + " " + tasks[e].endTime + "</p>" + a, $("#modalTitle").html(), $("#modalContent").html(str),
$("#modalSDate").html(), $("#modalEDate").html(), $("#myModal").foundation("reveal", "open")
} function unscheduledFire(t) {
    str = ""; var e = t.id - 1; chronological ? e -= tasks.length : e = e - trades.length - tasks.length;
    var a = "</p> <p><a class='primary hollow button small-six columns' href=" + unscheduled[e].action + ">Edit</a>"; str = str +
 " <div class='small-12 columns'><h2 class='lead'>" + unscheduled[e].name + "</h2><p>" + trades[unscheduled[e].trade - 1].name +
 "</p> </div><div class='small-12 columns'>" + unscheduled[e].content + "</div>" + a, $("#modalTitle").html(), $("#modalContent").html(str),
 $("#modalSDate").html(), $("#modalEDate").html(), $("#myModal").foundation("reveal", "open")
} function oldUnscheduledFire(t) {
    str = "<h2>" + trades[t - 1].name + "</h2>"; for (var e = unscheduled.length - 1; e >= 0; e--)
        if (unscheduled[e].trade == t) {
            var a = "</p> <p><a class='primary hollow button columns' href=" + unscheduled[e].action +
     ">Edit</a>"; str = str + startH + " <p class='lead'>" + unscheduled[e].name + "</p><p>" + unscheduled[e].content + "</p>" + middleH +
 "<p>" + unscheduled[e].contractor + "</p>" + a + endH
        } $("#modalContent").html(str), $("#modalSDate").html(" "), $("#modalEDate").html(" "), $("#modalTrade").html(" "),
 $("#myModal").foundation("reveal", "open")
} function collapseTasks() {
    chronological || (collapse ? (gantt.eachTask(function (t)
    { t.id < count && 0 == t.parent && (gantt.open(t.id), trades[t.id - 1].open = !0) }), collapse = !1) : (gantt.eachTask(function (t) {
        t.id < count
    && 0 == t.parent && (gantt.close(t.id), trades[t.id - 1].open = !1)
    }), collapse = !0), refresh(), collapse ? $("#collapse").html("Expand") : $("#collapse").html("Collapse"))
} function daysBetween(t, e) { var a = Math.abs(t.getTime() - e.getTime()); return Math.ceil(a / 864e5) } function fill()
{
    underline.length = 0, removeHeight = 0, count = 1; 0 == trades.length || 1 == chronological ? (addTasksGanttView(), addNoParentUnscheduled()) : (addTrades(),
    addTasks(), addUnscheduled()); var t = "javascript:__doPostBack('ctl00$ContentBody$lnkAddTask','')";
    $(".gantt_data_area").append("<a href='' id='gantt_add' class='right small alert button round'><sup>New Task</a>"), $("#gantt_add").attr("href", t),
height = 40 * (count - removeHeight), document.getElementById("gantt_here").setAttribute("style", "height:" + (window.innerHeight - 45) + "px")
} function addTrades() {
    for (var t in trades) load.data[t] = {
        id: count, text: trades[t].name, start_date: early, duration: 9999, parent: 0, color: trades[t].color,
        order: 10, open: trades[t].open, direction: ""
    }, color[count] = trades[t].color, count++
} function addTasks() {
    for (var t in tasks) {
        var e = tasks[t].start, a = tasks[t].end; "open" != e && "open" != a ? e > endOfWeek || startOfWeek > a ?
        (e > endOfWeek ? side = "rightArrow" : side = "leftArrow", load.data[count - 1] = {
            id: count, text: tasks[t].icon + " " +
    tasks[t].name, both: tasks[t].oneDate, unscheduled: !0, start_date: early, duration: 9999, color: color[tasks[t].trade], order: 10,
            parent: [tasks[t].trade], direction: side
        }) : (dur = daysBetween(e, a), markCells(tasks[t].trade, e, dur), load.data[count - 1] = {
            id: count, text: tasks[t].icon + " " +
     tasks[t].name, both: tasks[t].oneDate, unscheduled: !1, start_date: formatFunc(e), duration: dur + 1, color: color[tasks[t].trade], order: 10,
            parent: [tasks[t].trade], direction: ""
        }) : "open" == e ? (e = new Date(endOfWeek), e.setDate(e.getDate() - 1), dur = 2, startOfWeek > a ? (side = "leftArrow",
load.data[count - 1] = {
    id: count, text: tasks[t].icon + " " + tasks[t].name, both: tasks[t].oneDate, unscheduled: !0, start_date: early,
    duration: 9999, color: color[tasks[t].trade], order: 10, parent: [tasks[t].trade], direction: side
}) : endOfWeek > a ? (e = new Date(a), e.setDate(a.getDate() - 4), dur = daysBetween(e, a), side = "",
load.data[count - 1] = {
    id: count, text: tasks[t].icon + " " + tasks[t].name, both: tasks[t].oneDate, unscheduled: !1, start_date: formatFunc(e),
    duration: dur + 1, color: color[tasks[t].trade], order: 10, parent: [tasks[t].trade], direction: ""
}) : (side = "leftArrow", load.data[count - 1] = {
    id: count, text: tasks[t].icon + " " + tasks[t].name, both: tasks[t].oneDate, unscheduled: !1,
    start_date: formatFunc(e), duration: dur + 1, color: color[tasks[t].trade], order: 10, parent: [tasks[t].trade], direction: ""
})) : e > startOfWeek ? (a = new Date(e), a.setDate(e.getDate() - 3), dur = daysBetween(e, a), e > endOfWeek ? (side = "rightArrow",
 load.data[count - 1] = {
     id: count, text: tasks[t].icon + " " + tasks[t].name, both: tasks[t].oneDate, unscheduled: !0, start_date: early,
     duration: 9999, color: color[tasks[t].trade], order: 10, parent: [tasks[t].trade], direction: side
 }) : load.data[count - 1] = {
     id: count, text: tasks[t].icon + " " + tasks[t].name, both: tasks[t].oneDate, unscheduled: !1, start_date: formatFunc(e),
     duration: dur + 1, color: color[tasks[t].trade], order: 10, parent: [tasks[t].trade], direction: ""
 }) : (a = new Date(startOfWeek), dur = daysBetween(e, a), side = "leftArrow", load.data[count - 1] = {
     id: count, text: tasks[t].icon + " " + tasks[t].name,
     both: tasks[t].oneDate, unscheduled: !1, start_date: formatFunc(e), duration: dur + 1, color: color[tasks[t].trade], order: 10, parent: [tasks[t].trade],
     direction: ""
 }), count++
    }
} function addTasksGanttView() {
    tasks.sort(function (t, e) {
        return t = "open" != t.start ? t.start : t.end,
        e = "open" != e.start ? e.start : e.end, e > t ? -1 : t > e ? 1 : 0
    }), addSortedGantt(tasks)
} function addSortedGantt(t) {
    for (var e in t) {
        var a = t[e].start, n = t[e].end; "open" != a && "open" != n ? a > endOfWeek || startOfWeek > n ?
         (a > endOfWeek ? side = "rightArrow" : side = "leftArrow", load.data[count - 1] = {
             id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate,
             unscheduled: !0, start_date: early, duration: 9999, color: color[t[e].trade], order: count, parent: 0, direction: side
         }) : (dur = daysBetween(a, n), markCells(t[e].trade, a, dur), load.data[count - 1] = {
             id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate,
             unscheduled: !1, start_date: formatFunc(a), duration: dur + 1, color: color[t[e].trade], order: count, parent: 0, direction: ""
         }) : "open" == a ? (a = new Date(endOfWeek), a.setDate(a.getDate() - 1), dur = 2, startOfWeek > n ? (side = "leftArrow",
load.data[count - 1] = {
    id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate, unscheduled: !0, start_date: early, duration: 9999,
    color: color[t[e].trade], order: count, parent: 0, direction: side
}) : endOfWeek > n ? (a = new Date(n), a.setDate(n.getDate() - 4), dur = daysBetween(a, n), side = "",
load.data[count - 1] = {
    id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate, unscheduled: !1, start_date: formatFunc(a), duration: dur + 1,
    color: color[t[e].trade], order: count, parent: 0, direction: ""
}) : (side = "leftArrow", load.data[count - 1] = {
    id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate, unscheduled: !1, start_date: formatFunc(a),
    duration: dur + 1, color: color[t[e].trade], order: count, parent: 0, direction: ""
})) : a > startOfWeek ? (n = new Date(a), n.setDate(a.getDate() - 3), dur = daysBetween(a, n), a > endOfWeek ? (side = "rightArrow",
load.data[count - 1] = {
    id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate, unscheduled: !0,
    start_date: early, duration: 9999, color: color[t[e].trade], order: count, parent: 0, direction: side
}) : load.data[count - 1] = {
    id: count, text: t[e].icon + " " + t[e].name, both: t[e].oneDate, unscheduled: !1,
    start_date: formatFunc(a), duration: dur + 1, color: color[t[e].trade], order: count, parent: 0, direction: ""
}) : (n = new Date(startOfWeek), dur = daysBetween(a, n), side = "leftArrow", load.data[count - 1] = {
    id: count, text: t[e].icon + " " + t[e].name,
    both: t[e].oneDate, unscheduled: !1, start_date: formatFunc(a), duration: dur + 1, color: color[t[e].trade], order: count, parent: 0, direction: ""
}), count++
    }
} function addTasksChronologically() {
    for (var t in tasks) {
        var e = tasks[t].start, a = tasks[t].end;
        "open" != e && "open" != a && (e > endOfWeek || startOfWeek > a ? (e > endOfWeek ? side = "rightArrow" : side = "leftArrow",
 load.data[count - 1] = {
     id: count, text: tasks[t].name, unscheduled: !0, start_date: early, duration: 9999, color: color[tasks[t].trade],
     order: 40, direction: side
 }) : (dur = daysBetween(e, a), markCells(tasks[t].trade, e, dur), load.data[count - 1] = {
     id: count, text: tasks[t].name, unscheduled: !1,
     start_date: formatFunc(e), duration: dur + 1, color: color[tasks[t].trade], order: 40, direction: ""
 }), count++)
    }
} function addGroupedUnscheduled()
{
    for (var t = [], e = unscheduled.length - 1; e >= 0; e--) isNaN(t[unscheduled[e].trade]) ? t[unscheduled[e].trade] = 1 : t[unscheduled[e].trade]++;
    for (var e = t.length - 1; e >= 0; e--) "undefined" != typeof t[e] && (load.data[count - 1] = {
        id: count, text: "+ " + t[e] + " <b><u>U</u></b>",
        unscheduled: !1, start_date: startOfWeek, duration: 1, color: "unscheduled " + color[e], order: 50, parent: e
    }, count++)
} function addUnscheduled() {
    for (var t in unscheduled) load.data[count - 1] = {
        id: count, text: unscheduled[t].icon + " <b><u>U</u></b> " + unscheduled[t].name,
        unscheduled: !1, start_date: startOfWeek, duration: 1, color: "unscheduled " + color[unscheduled[t].trade], order: 50, parent: unscheduled[t].trade
    }, count++
} function addNoParentUnscheduled() {
    for (var t in unscheduled) load.data[count - 1] = {
        id: count, text: " <b><u>U</u></b> " + unscheduled[t].name,
        unscheduled: !1, start_date: startOfWeek, duration: 1, color: "unscheduled " + color[unscheduled[t].trade], order: 50, parent: 0
    }, count++
} function markCells(t, e, a) {
    var n = daysBetween(startOfWeek, e), r = underline[t]; null == r && (r = []), startOfWeek > e && (a = a - n + 1, n = 0);
    for (var o = 0; n > o; o++); for (var o = 0; a >= o; o++) r[n + o] = !0; for (; date_range >= o; o++); underline[t] = r
} function underlineCells(t, e) {
    var a = underline[t]; if (void 0 != a) for (var n = 0;
     n < a.length; n++) a[n] && $(".gantt_task_row.parent_row." + e).children().eq(n).addClass("shade")
} function underlineTasks() { for (var t = underline.length - 1; t > 0; t--) task = gantt.getTask(t), task.open || underlineCells(t, task.color) }
function refresh() {
    gantt.clearAll(), load.data.length = 0, gantt.init("gantt_here"), fill(), gantt.parse(load),
     gantt.render(), $(".gantt_scale_line:eq( 1 )").addClass("scale_gone"), $(".gantt_scale_line:eq( 0 )").addClass("scale_stay"), chronological ? arrowsDark() : (underlineTasks(), arrows()), timeMark(), daysBetween(startOfWeek, endOfWeek) > 7 ? $("#time").html("<p><i class='fi-zoom-in'></i></p>") : $("#time").html("-")
} function arrows() {
    $(".rightArrow .gantt_last_cell").each(function (t) { $(this).html("<sup><i class='fi-arrow-right'></i></sup> ") }),
     $(".leftArrow").each(function (t) { $(this).children().eq(0).html("<sup><i class='fi-arrow-left'></i></sup> ") })
} function arrowsDark() {
    $(".rightArrow .gantt_last_cell").each(function (t) { $(this).html("<sup><i class='fi-arrow-right'></i></sup> ") }),
     $(".leftArrow").each(function (t) { $(this).children().eq(0).html("<sup><i class='fi-arrow-left'></i></sup> ") })
} function dateReorg(t) { var e = t.substring(0, 2), a = t.substring(3, 5), n = t.substring(6, 10); return new Date(a + "/" + e + "/" + n) }
function setTrade(t, e) {
    if (t = t + " - " + e, tradeCat.length > 0) {
        for (var a = tradeCat.length; a >= 0; a--) if (tradeCat[a] == t) return a;
        return tradeCat[tradeCat.length] = t, tradeCat.length - 1
    } return tradeCat[0] = t, 0
} function grabTasks()
{
    $('[id*="SelectJob"]').each(function (t) {
        $(this).hasClass("green") || $(this).hasClass("grey") || $(this).hasClass("redTask") ? addToList(this, t,
         "<i class='fi-checkbox'></i>", "complete") : addToList(this, t, "<i class='fi-minus'></i>", "incomplete")
    }), tasks.sort(function (t, e) { return t = t.start, e = e.start, e > t ? -1 : t > e ? 1 : 0 }); for (var t = tradeCat.length - 1;
 t >= 0; t--) trades[t] = { name: tradeCat[t], open: !0, color: listOfColors[t % 7] }
} function addToList(t, e, a, n) {
    var r = $(t).text(); r = r.replace(/^\s+|\s+$/g, "").replace(/\s\s+/g, " ");
    var o = r.substring(0, r.indexOf("Starts") - 1), d = r.substring(r.indexOf("Starts") + 8, r.indexOf("Starts") + 18); d = dateReorg(d);
    var s = r.substring(r.indexOf("Ends") + 6, r.indexOf("Ends") + 16); s = dateReorg(s); var l = r.substring(r.indexOf("ame:") + 5),
i = r.substring(r.indexOf("ontractor:") + 11, r.indexOf("Trade") - 1), c = $(t).parent().next().children().text(), u = $(t).attr("href");
    if ("Invalid Date" == d && "Invalid Date" == s) unscheduled[unscheduled.length] = {
        content: c, name: o, icon: "", tradeName: l, trade: setTrade(l, i) + 1,
        action: u, contractor: i
    }; else {
        var f = r.substring(r.indexOf("\n"), r.indexOf("Contractor")), h = f.substring(f.indexOf("Starts") + 18, f.indexOf("|")),
     g = f.substring(f.indexOf("|") + 18); h.indexOf(":") < 0 && (h = ""), g.indexOf(":") < 0 && (g = ""); var k = "qboth"; "Invalid Date" == d && (k = "qright",
 d = "open"), "Invalid Date" == s && (k = "qleft", s = "open"), k = k + " " + n, tasks[tasks.length] = {
     content: c, name: o, icon: a, start: d, startTime: h,
     end: s, endTime: g, tradeName: l, trade: setTrade(l, i) + 1, action: u, contractor: i, oneDate: k
 }
    }
} function timeMark() {
    var t = $(".gantt_task_cell.today").width() / 24, e = today.getHours(); t *= e,
     $(".gantt_task_cell.today:first").append("<div style='margin-left:" + t + "px!important' class=' line right alert '></div>"),
$(".gantt_task_row:last").css("color", "red")
} gantt.config.show_unscheduled = !0, gantt.config.show_grid = !1, gantt.config.drag_resize = !1, gantt.config.drag_progress = !1,
 gantt.config.drag_move = !1, gantt.config.drag_links = !1, gantt.config.min_column_width = 1;
var buttonGrid = "<ul class='button-group round even-7 gchart' ><li><a class='button' onclick='left()'><i class='fi-arrow-left'></i></a></li><li><a id=collapseButton class='secondary hollow button' onclick='collapseTasks()'><p id='collapse'>Collapse</p></a></li><li><a class='secondary hollow button' onclick='toToday()'><p id='today'>Today</p></a></li><li><a class='secondary hollow button' onclick='chronologicalToggle()'><p id='chrono'>Gantt&nbspview</p></a></li><li><a class='secondary hollow button' onclick='toggleIn()'><p id='time'>-</p></a></li><li><a class='secondary hollow button' onclick='toggleOut()'><p><i class='fi-zoom-out'></i></p></a></li><li><a class='button' onclick='right()'><i class='fi-arrow-right'></i></a></ul><div id='gantt_here' style=' width:300px; height:100px;'></div></div>";
$("#gantt_here").parent().html(buttonGrid); var date_range = 6, today = new Date, startOfWeek = getMonday(new Date),
endOfWeek = new Date(startOfWeek.setDate(startOfWeek.getDate() + date_range)); startOfWeek = getMonday(new Date),
gantt.config.start_date = startOfWeek, gantt.config.end_date = endOfWeek; var zoom = !0, collapse = !1, chronological = !1,
str = "", startH = "<br /><div class='row'><div class='small-12 medium-6 columns'>", middleH = "</div><div class='small-12 medium-6 columns'>",
 endH = "</div></div> </br>"; gantt.templates.scale_cell_class = function (t) {
     if (zoom) {
         if (t.toDateString() == today.toDateString()) return "Today";
         if (0 == t.getDay() || 6 == t.getDay()) return "weekend"
     } else if (t = new Date(t), t < new Date(today) && t.setDate(t.getDate() + 7) > new Date(today)) return "Today"
 }, gantt.templates.task_cell_class = function (t, e, a) { return e.toDateString() == today.toDateString() ? "Today" : void 0 },
gantt.templates.task_class = function (t, e, a) {
    return a.parent > 0 ? (parent = gantt.getTask(a.parent),
    parent.shrunk ? a.color + " shrunk " + a.both : a.color + " big " + a.both)
: chronological ? a.direction + " chrono_row " + a.color + " shrunk " + a.both : a.color + " project"
}, gantt.templates.task_row_class = function (t, e, a)
{ return chronological ? a.direction + " chrono_row" : a.parent > 0 ? a.color + " lighten " + a.direction : a.color + " parent_row" };
 var formatFunc = gantt.date.date_to_str("%d/%m/%Y"), color = [],
 listOfColors = ["gantt_blue", "gantt_purple", "gantt_green", "gantt_red", "gantt_orange", "gantt_pink", "gantt_yellow"], underline = [],
duration, early = new Date, count, removeHeight; early.setYear(early.getFullYear() - 1); var load = { data: [], links: [] }, tradeCat = [],
tasks = [], oldtasks = [{ content: "Description of task", name: "Task One Name", start: "02-09-2016", end: "02-11-2016", trade: 1 },
 { content: "Description of task", name: "Task Two", start: "02-09-2016", end: "02-14-2016", trade: 3 }, {
     content: "Description of task",
     name: "Task Three", start: "02-11-2016", end: "02-13-2016", trade: 2
 }, { content: "Description of task", name: "Task Four", start: "02-01-2016", end: "02-14-2016", trade: 2 }, {
     content: "Description of task",
     name: "Task six", start: "02-1-2016", end: "02-6-2016", trade: 2
 }, { content: "Description of task", name: "Task seven", start: "02-18-2016", end: "02-22-2016", trade: 3 }, {
     content: "Description of task",
     name: "Task Four", start: "02-01-2017", end: "02-14-2017", trade: 2
 }, { content: "Description of task", name: "Task eight", start: "02-01-2015", end: "02-14-2015", trade: 2 }, {
     content: "Description of task",
     name: "Task Five", start: "01-11-2016", end: "02-13-2016", trade: 2
 }], trades = [], unscheduled = [], oldUnscheduled = [{ name: "Unscheduled One", trade: 1 }, { name: "Unscheduled Two", trade: 1 },
{ name: "Unscheduled Three", trade: 2 }, { name: "Unscheduled Four", trade: 2 }, { name: "Unscheduled Five", trade: 1 },
 { name: "Unscheduled Six", trade: 3 }, { name: "Unscheduled Seven", trade: 3 }, { name: "Unscheduled Eight", trade: 1 }];
grabTasks(), gantt.init("gantt_here"), fill(), gantt.parse(load), underlineTasks(), arrows(), mediaQuery(), gantt.attachEvent("onTaskDblClick",
 function (t, e) {
     task = gantt.getTask(t), 0 == task.parent && 40 != task.order ? gantt.getTask(t).open ? (gantt.close(t), trades[task.id - 1].open = !1,
     refresh()) : (gantt.open(task.id), trades[task.id - 1].open = !0, refresh()) : 50 == task.order ? unscheduledFire(task) : taskFired(task)
 }), gantt.attachEvent("onTaskClick",
function (t, e) { task = gantt.getTask(t), 0 != task.parent || 40 == task.order || chronological ? 50 == task.order ? unscheduledFire(task) : taskFired(task) : gantt.getTask(t).open ? (gantt.close(t), trades[task.id - 1].open = !1, refresh()) : (gantt.open(task.id), trades[task.id - 1].open = !0, refresh()) }),
 $(window).resize(function () { refresh() }), timeMark();
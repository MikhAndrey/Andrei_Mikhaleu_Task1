const getTextOfDurationAndDistance = (duration, distance) => {
    const distanceText = `${distance / 1000} км`;

    const SECONDS_IN_YEAR = 31536000;
    const SECONDS_IN_MONTH = 2592000;
    const SECONDS_IN_DAY = 86400;
    const SECONDS_IN_HOUR = 3600;
    const SECONDS_IN_MINUTE = 60;

    const years = Math.floor(duration / SECONDS_IN_YEAR);   
    const months = Math.floor((duration % SECONDS_IN_YEAR) / SECONDS_IN_MONTH);
    const days = Math.floor((duration % SECONDS_IN_MONTH) / SECONDS_IN_DAY);
    const hours = Math.floor((duration % SECONDS_IN_DAY) / SECONDS_IN_HOUR);
    const minutes = Math.floor((duration % SECONDS_IN_HOUR) / SECONDS_IN_MINUTE);

    const yearsText = years > 0 ? years + " г. " : "";
    const monthsText = months > 0 ? months + " м. " : "";
    const daysText = days > 0 ? days + " д. " : "";
    const hoursText = hours > 0 ? hours + " ч. " : "";
    const minutesText = minutes + " мин.";
    return {
        duration: yearsText + monthsText + daysText + hoursText + minutesText,
        length: distanceText
    };
};
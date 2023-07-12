export const formatDuration = (durationInHours: number): string => {
  const SECONDS_IN_YEAR = 31536000;
  const SECONDS_IN_MONTH = 2592000;
  const SECONDS_IN_DAY = 86400;
  const SECONDS_IN_HOUR = 3600;
  const SECONDS_IN_MINUTE = 60;

  const duration = Math.floor(durationInHours * SECONDS_IN_HOUR);

  const years = Math.floor(duration / SECONDS_IN_YEAR);
  const months = Math.floor((duration % SECONDS_IN_YEAR) / SECONDS_IN_MONTH);
  const days = Math.floor((duration % SECONDS_IN_MONTH) / SECONDS_IN_DAY);
  const hours = Math.floor((duration % SECONDS_IN_DAY) / SECONDS_IN_HOUR);
  const minutes = Math.floor((duration % SECONDS_IN_HOUR) / SECONDS_IN_MINUTE);

  const yearsText = years > 0 ? years + " years " : "";
  const monthsText = months > 0 ? months + " months " : "";
  const daysText = days > 0 ? days + " days " : "";
  const hoursText = hours > 0 ? hours + " hours " : "";
  const minutesText = minutes + " mins";
  return yearsText + monthsText + daysText + hoursText + minutesText;
}

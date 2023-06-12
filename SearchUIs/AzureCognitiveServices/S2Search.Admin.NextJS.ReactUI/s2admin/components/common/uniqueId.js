export default function uniqueId(prefix) {
  return `${prefix}_${Math.random().toString(16).slice(2)}`;
}

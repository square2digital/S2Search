interface LogDetailsProps {
  enable?: boolean;
  logData?: Record<string, any>;
  [key: string]: any;
}

export const LogDetails = (props: LogDetailsProps): null => {
  if (process.env.NODE_ENV !== 'production' && props.enable) {
    logProps(props);
  }

  return null;
};

const logProps = (props: LogDetailsProps): void => {
  if (props.logData) {
    for (const key in props.logData) {
      // eslint-disable-next-line no-console
      console.dir(
        `${key} - ${
          props.logData[key] ? JSON.stringify(props.logData[key]) : 'empty'
        }`
      );
    }
  }
};
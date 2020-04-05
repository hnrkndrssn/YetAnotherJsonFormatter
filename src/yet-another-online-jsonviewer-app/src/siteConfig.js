"use strict";

import { version } from '../package';

const siteConfig = {
    version,
    apiUrl: process.env.VUE_APP_YAJSONFMT_APIURL || 'https://api.yajsonfmt.app',
};

export default siteConfig;
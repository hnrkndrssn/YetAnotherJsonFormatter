<template>
  <v-app :dark="nightmode">
    <v-navigation-drawer
      v-model="infoDrawer.model"
      :clipped="true"
      width="600"
      temporary
      absolute
      app>
      <v-toolbar flat class="transparent">
        <v-list>
          <v-list-tile>
            <v-list-tile-title class="title">
              About <AppTitle />
            </v-list-tile-title>
          </v-list-tile>
        </v-list>
      </v-toolbar>
      <v-divider></v-divider>
      <blockquote class="blockquote pr-2">
        Many websites which offer API's, will return data in JSON format. 
        Often the JSON provided has white space compressed to reduce the size of the data transferred. 
        This site gives you a quick and easy way to format the JSON so you can read it.
      </blockquote>
      <blockquote class="blockquote pr-2">
        <AppTitle /> also provides an API that you can call directly to format the JSON in a more readable format.
      </blockquote>
      <v-list dense>
        <v-list-group
          no-action
        >
          <template v-slot:activator>
            <v-list-tile>
              <v-list-tile-title class="subheading">API Reference</v-list-tile-title>
            </v-list-tile>
          </template>

          <blockquote class="blockquote">
              All API access is over <strong>HTTPS</strong>, and accessed from <kbd>{{config.apiUrl}}</kbd>. All data is sent and received as <strong>JSON</strong>.
          </blockquote>

          <blockquote class="blockquote"
            v-for="(endpoint, i) in api.endpoints"
            :key="i">
            <span class="text-capitalize font-weight-light headline">{{endpoint.name}}</span>
            <p>
              <kbd style="width: 96%" class="pa-2 mt-2">{{endpoint.method}} {{endpoint.uri}}</kbd>
            </p>
            <span class="text-capitalize font-weight-light">Example</span>
            <p class="body-1 mr-4">
              <kbd class="pl-2">
{{endpoint.example}}
              </kbd>
            </p>
            <div
              v-if="endpoint.params.length > 0"
            >
              <span class="text-capitalize font-weight-light">
                Parameters
              </span>
              <table class="body-1">
                <thead>
                  <th>Name</th>
                  <th>Type</th>
                  <th>Description</th>
                </thead>
                <tbody>
                  <tr
                    v-for="(param, i) in endpoint.params"
                    :key="i"
                  >
                    <td class="text-xs-center pl-2 pr-2"><code>{{param.name}}</code></td>
                    <td class="text-xs-center pl-2 pr-2"><code>{{param.type}}</code></td>
                    <td v-html="`${param.description}${(param.default ? `<br/>Default: <code>${param.default}</code>` : '')}`" class="pa-2"></td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div
              v-if="endpoint.response"
              >
              <span class="text-capitalize font-weight-light">
                Response
              </span>
              <p class="body-1">
              <kbd class="pl-2 pr-2" style="width: 96%">
{{JSON.parse(endpoint.response)}}
              </kbd>
              </p>
            </div>
          </blockquote>
        </v-list-group>
      </v-list>    
    </v-navigation-drawer>
    <v-navigation-drawer
      v-model="preferencesDrawer.model"
      temporary
      absolute
      app>
      <v-toolbar flat class="transparent">
        <v-list>
          <v-list-tile>
            <v-list-tile-title class="title">
              Preferences
            </v-list-tile-title>
          </v-list-tile>
        </v-list>
      </v-toolbar>
      <v-divider></v-divider>
      <v-list 
        subheader>
        <v-subheader>Tabs</v-subheader>
        <v-list-tile
          key="TabWidth"
          >
          <v-list-tile-action>
            <v-select
              :items="settings.tabWidth.widths"
              v-model="settings.tabWidth.width"
              label="Tab width"
            ></v-select>
          </v-list-tile-action>
        </v-list-tile>
        <v-list-tile
          key="UseTabs">
          <v-list-tile-action>
            <v-checkbox
              v-model="settings.useTabs"
              label="Use tabs"
              color="primary"
            ></v-checkbox>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
      <v-divider></v-divider>
      <v-list subheader>
        <v-subheader>Quotes</v-subheader>
        <v-list-tile
          key="QuoteChar">
          <v-list-tile-action>
            <v-select
              :items="settings.quoteChars.options"
              v-model="settings.quoteChars.option"
              label="Quote character"
            ></v-select>
          </v-list-tile-action>
        </v-list-tile>
        <v-list-tile
          key="QuoteNames">
          <v-list-tile-action>
            <v-checkbox
              v-model="settings.quoteNames"
              label="Quote object names"
              color="primary"
            ></v-checkbox>
          </v-list-tile-action>
        </v-list-tile>
        <v-list-tile
          key="MinifyJson"
          >
          <v-list-tile-action>
            <v-checkbox
              v-model="settings.minify"
              label="Minify result"
              color="primary"
            ></v-checkbox>
          </v-list-tile-action>
        </v-list-tile>
        <v-divider></v-divider>
        <v-list-tile
          key="NightMode">
          <v-list-tile-action>
            <v-switch
              v-model="nightmode"
              label="Night mode"
              hide-details
              color="primary"
            ></v-switch>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>
    <v-toolbar
      dense
      flat
      absolute
      app>
      <v-btn small flat @click="infoDrawer.model = !infoDrawer.model" color="primary" class="font-weight-light"><v-icon small>info</v-icon>&nbsp;Info</v-btn>
      <v-btn small flat @click="preferencesDrawer.model = !preferencesDrawer.model" color="primary" class="font-weight-light"><v-icon small>settings</v-icon>&nbsp;Preferences</v-btn>
      <v-spacer></v-spacer>
      <v-toolbar-title class="mr-5 align-center">
        <AppTitle :version="config.version"/>
      </v-toolbar-title>
    </v-toolbar>
    <v-content app>
      <v-container fluid fill-height>
        <Editors :nightmode="nightmode" :settings="settings" :apiUrl="config.apiUrl" />
      </v-container>
    </v-content>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import AppTitle from './components/AppTitle.vue';
import Editors from './components/Editors.vue';

@Component({
    components: {
      AppTitle,
      Editors,
    },
    data: () => ({
      nightmode: true,
      infoDrawer: {
        model: false,
      },
      preferencesDrawer: {
        model: false,
      },
      settings: {
        tabWidth: {
          widths: [1, 2, 3, 4],
          width: 2,
        },
        useTabs: false,
        quoteChars: {
          options: [
            {text: 'Single quote (\')', value: '\''},
            {text: 'Double quote (")', value: '"'},
          ],
          option: '"',
        },
        quoteNames: true,
        minify: false,
      },
      api: {
        endpoints: [
          {
            name: 'Format JSON',
            uri: '/json',
            method: 'POST',
            params: [
              {
                name: 'indent',
                default: '2',
                type: 'int',
                description: 'How many <code>indentChar</code> to write for each level in the hierarchy when <code>compact</code> is set to <code>false</code>',
              },
              {
                name: 'indentChar',
                default: ' ',
                type: 'char',
                description: 'Which character to use for indenting when <code>compact</code> is set to <code>false</code>',
              },
              {
                name: 'compact',
                default: 'false',
                type: 'boolean',
                description: 'Value indicating how JSON text output should be formatted.',
              },
              {
                name: 'validate',
                default: 'true',
                type: 'boolean',
                description: 'Validate that the JSON sent is valid.',
              },
              {
                name: 'quoteChar',
                default: '"',
                type: 'char',
                description: 'Which character to use to quote attribute values.',
              },
              {
                name: 'quoteNames',
                default: 'true',
                type: 'boolean',
                description: 'Value indicating whether object names will be surrounded with quotes',
              },
            ],
            example: '{"firstName":"John","lastName":"Smith","isAlive":true,"age":27,"address":{"streetAddress":"21 2nd Street","city":"New York","state":"NY","postalCode":"10021-3100"},"phoneNumbers":[{"type":"home","number":"212 555-1234"},{"type":"office","number":"646 555-4567"},{"type":"mobile","number":"123 456-7890"}],"children":[],"spouse":null}',
            response: '{"firstName":"John","lastName":"Smith","isAlive":true,"age":27,"address":{"streetAddress":"21 2nd Street","city":"New York","state":"NY","postalCode":"10021-3100"},"phoneNumbers":[{"type":"home","number":"212 555-1234"},{"type":"office","number":"646 555-4567"},{"type":"mobile","number":"123 456-7890"}],"children":[],"spouse":null}',
          },
          {
            name: 'Validate JSON',
            uri: '/json/validate',
            method: 'POST',
            params: [],
            example: '{"firstName":"John","lastName":"Smith","isAlive":true,"age":27,"address":{"streetAddress":"21 2nd Street","city":"New York","state":"NY","postalCode":"10021-3100"},"phoneNumbers":[{"type":"home","number":"212 555-1234"},{"type":"office","number":"646 555-4567"},{"type":"mobile","number":"123 456-7890"}],"children":[],"spouse":null}',
            response: '{ "success": true, "message": "Input is valid JSON" }',
          },
        ],
      },
      config: require('./siteConfig').default,
    }),
})

export default class App extends Vue {}
</script>

<style scoped>
.title {
  font-weight: 300;
}

.about {
  padding: 10px!important;
  font-weight: 300;
}

.version {
  font-size: 10px!important;
}
</style>
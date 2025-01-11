import { AUTH_EVENTS, initialize } from '@bcwdev/auth0provider-client'
import { AppState } from '../AppState.js'
import { audience, clientId, domain } from '../env.js'
import { accountService } from './AccountService.js'
import { api } from './AxiosService.js'
import { socketService } from './SocketService.js'
import Pop from '@/utils/Pop.js'



export const AuthService = initialize({
  domain,
  clientId,
  authorizationParams: {
    audience,
    redirect_uri: window.location.origin,
  }
})

AuthService.on(AUTH_EVENTS.AUTHENTICATED, async function () {
  api.defaults.headers.authorization = AuthService.bearer
  api.interceptors.request.use(refreshAuthToken)
  AppState.identity = AuthService.identity
  await accountService.getAccount()
  socketService.authenticate(AuthService.bearer)
  // NOTE if there is something you want to do once the user is authenticated, place that here
  try {
    await accountService.getAccountBook()
    await accountService.getLikedPoemsByProfileId()
  }
  catch (error) {
    Pop.error(error);
  }
})

async function refreshAuthToken(config) {
  if (AuthService.state == AUTH_EVENTS.AUTHENTICATED) { return config }
  const expires = AuthService.identity.exp * 1000
  const expired = expires < Date.now()
  const needsRefresh = expires < Date.now() + (1000 * 60 * 60 * 12)
  if (expired) {
    await AuthService.loginWithPopup()
  } else if (needsRefresh) {
    await AuthService.getTokenSilently()
    api.defaults.headers.authorization = AuthService.bearer
    socketService.authenticate(AuthService.bearer)
  }
  return config
}

